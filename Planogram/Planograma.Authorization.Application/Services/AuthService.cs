using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Planograma.Authorization.Application.Models.Users;
using Planograma.Authorization.Application.Authorization;
using Planograma.Authorization.Application.Helpers;
using Planograma.Authorization.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planograma.EmplUser.Application.Models.Users;
using Planograma.Authorization.Application.Interfaces;

namespace Planograma.Authorization.Application.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse >Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        Task<IEnumerable<EmployeeParams>> GetAll();
        Task<EmployeeParams> GetById(int id);
        string CreatePasswordHash(string password);
    }

    public class AuthService : IUserService
    {
        private IApplicationAuthDbContext _context;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public AuthService(
            IApplicationAuthDbContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = await _context.EmployeeParams.SingleOrDefaultAsync(x => x.Username == model.Username);

            var passwordVerify = BCryptNet.Verify(model.Password, user.PasswordHash);
             if (user == null || !passwordVerify)
            {
                await AuthenticationInfoAccounting(user.EmployeeId, ipAddress, passwordVerify);
                throw new AppException("Username or password is incorrect");
            }

            await AuthenticationInfoAccounting(user.EmployeeId,ipAddress, passwordVerify);

            var jwtToken =await _jwtUtils.GenerateJwtToken(user.EmployeeId);
            var refreshToken = await _jwtUtils.GenerateRefreshToken(ipAddress);
            user.addRefreshTokens(refreshToken);

            
            removeOldRefreshTokens(user);

           
            _context.EmployeeParams.Update(user);

            await _context.SaveChangesAsync();

            return new AuthenticateResponse(user.EmployeeId,user.Username, jwtToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var user =await getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _context.EmployeeParams.Update(user);
                await _context.SaveChangesAsync(default);
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken =await rotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _context.EmployeeParams.Update(user);
            await _context.SaveChangesAsync(default);

            // generate new jwt
            var jwtToken =await _jwtUtils.GenerateJwtToken(user.EmployeeId);

            return new AuthenticateResponse(user.EmployeeId, user.Username,jwtToken, newRefreshToken.Token);
        }

        public async void RevokeToken(string token, string ipAddress)
        {
            var user = await getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _context.EmployeeParams.Update(user);
            await _context.SaveChangesAsync(default);
        }

        public async Task<IEnumerable<EmployeeParams>> GetAll()
        {
            return await _context.EmployeeParams.ToListAsync();
        }

        public async Task<EmployeeParams> GetById(int id)
        {
            var user = await _context.EmployeeParams.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        

        private async Task<EmployeeParams> getUserByRefreshToken(string token)
        {
            var user =await _context.EmployeeParams.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                throw new AppException("Invalid token");

            return user;
        }

        private async Task<RefreshToken> rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken =await _jwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(EmployeeParams user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x => 
                !x.IsActive && 
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, EmployeeParams user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        public string CreatePasswordHash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            return  BCryptNet.HashPassword(password);
            /*using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }*/
        }

        private async Task AuthenticationInfoAccounting(int employeeId, string ipAddress,bool isSuccesLoggedIn)
        {
            var entity = new AuthenticationInfo();
            if (await _context.AuthenticationInfos
                .AnyAsync(x => x.EmployeeId == employeeId 
                            && x.IsDelete == false 
                            && x.IsBlockActive == true 
                          )
                )
            {
                throw new AppException("Account is blocked, Please Contact Admin");
            }

            var authInfos = await _context.AuthenticationInfos
                .OrderByDescending(x => x.Id)
                .Where(x => x.EmployeeId == employeeId && x.IsDelete == false && x.IsBlockIPAdressActive == true && x.IsBlockActive == false)
                .ToListAsync();

            if (authInfos.Count > 1)
            {
                entity.EmployeeId = employeeId;
                entity.ConnectedIP = ipAddress;
                entity.IsBlockActive = true;
                entity.IsBlockIPAdressActive = true;
                entity.ConnectedTime = DateTime.UtcNow;
                entity.IsSuccesLoggedIn = false;
            }
            else
            {


                var authInfosByIP = await _context.AuthenticationInfos
                    .Where(x=>x.EmployeeId == employeeId && x.ConnectedIP == ipAddress && x.IsDelete == false)
                    .OrderByDescending(x=>x.Id)
                    .Take(3)
                    .ToListAsync();
            
                if (authInfosByIP.Count == 3 &&
                    authInfosByIP.Where(x=>x.IsSuccesLoggedIn == false).Count() == 3)
                {
                    if (authInfosByIP.Any(x=> x.IsBlockIPAdressActive == true))
                    {
                        throw new AppException("Account is blocked, Please Contact Admin");
                    }
                    var timeDifference = authInfosByIP.First().ConnectedTime.Subtract(authInfosByIP.Last().ConnectedTime).TotalHours;
                    if (timeDifference < 1)
                    {
                    
                        entity.EmployeeId = employeeId;
                        entity.ConnectedIP = ipAddress;
                        entity.IsBlockActive = false;
                        entity.IsBlockIPAdressActive = true;
                        entity.ConnectedTime = DateTime.UtcNow;
                        entity.IsSuccesLoggedIn = false;
                    
                    }
                }
                else
                {
                    entity.EmployeeId = employeeId;
                    entity.ConnectedIP = ipAddress;
                    entity.IsBlockActive = false;
                    entity.IsBlockIPAdressActive = false;
                    entity.ConnectedTime = DateTime.UtcNow;
                    entity.IsSuccesLoggedIn = isSuccesLoggedIn;
                }
            }

            await _context.AuthenticationInfos.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}