using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Planograma.Authorization.Application.Helpers;
using Planograma.Authorization.Application.Services;
using Planograma.Authorization.Domain.Entities;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace Planograma.Authorization.Application.Authorization
{
    public interface IJwtUtils
    {
        public Task<string> GenerateJwtToken(int userId);
        public Task<int> ValidateJwtToken(string token);
        public Task<RefreshToken> GenerateRefreshToken(string ipAddress);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        private readonly IRoleService _roleService;

        public JwtUtils(IOptions<AppSettings> appSettings, IRoleService roleService)
        {
            _appSettings = appSettings.Value;
            _roleService = roleService;
        }

        public async Task<string> GenerateJwtToken(int userId)
        {
            // generate token that is valid for 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var userRoles = await _roleService.GetRolesAsync(userId);
            //var test = _roleService.GetAllActionMethodAsync();
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                };
            if (userRoles != null)
            {
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }
            var appIdentity = new ClaimsIdentity(authClaims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = appIdentity,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task<int> ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return Task.FromResult(userId);
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            // generate token that is valid for 7 days
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return Task.FromResult(refreshToken);
        }

    }
}