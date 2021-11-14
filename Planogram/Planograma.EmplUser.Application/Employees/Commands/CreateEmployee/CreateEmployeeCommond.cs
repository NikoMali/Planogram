using MediatR;
using Microsoft.EntityFrameworkCore;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Application.Services;
using Planograma.Authorization.Domain.Entities;
using Planograma.EmplUser.Application.Helpers.Exceptions;
using Planograma.EmplUser.Application.Interfaces;
using Planograma.EmplUser.Domain.Entities;
using Planograma.EmplUser.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommond : IRequest<CreateEmployeeCommond>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string MobileNumber { get; set; }
        public string password { get; set; }
    }

    public class CreateEmployeeCommondHandler : IRequestHandler<CreateEmployeeCommond, CreateEmployeeCommond>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationAuthDbContext _authContext;
        private readonly IUserService _userService;

        public CreateEmployeeCommondHandler(
            IApplicationDbContext context,
            IApplicationAuthDbContext authContext,
            IUserService userService
            )
        {
            _context = context;
            _authContext = authContext;
            _userService = userService;
        }

        public async Task<CreateEmployeeCommond> Handle(CreateEmployeeCommond request, CancellationToken cancellationToken)
        {
            if (await _authContext.EmployeeParams.AnyAsync(x=>x.Username == request.UserName))
            {
                throw new ValidationException("UserName Already Used");
            }
            var entity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Avatar = request.Avatar,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                Done = false
            };
            entity.DomainEvents.Add(new EmployeeCreatedEvent(entity));

            await _context.Employees.AddAsync(entity);

            await _context.SaveChangesAsync(cancellationToken);
            var entityParam = new EmployeeParams
            {
                EmployeeId = entity.Id,
                Username = request.UserName,
                PasswordHash = _userService.CreatePasswordHash(request.password)
            };
            await _authContext.EmployeeParams.AddAsync(entityParam);
            await _context.SaveChangesAsync(cancellationToken);
            return request;
        }
    }
}
