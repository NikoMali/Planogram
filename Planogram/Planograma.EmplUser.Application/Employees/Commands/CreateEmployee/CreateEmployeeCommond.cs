using MediatR;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Application.Services;
using Planograma.Authorization.Domain.Entities;
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
    public class CreateEmployeeCommond : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
    }

    public class CreateEmployeeCommondHandler : IRequestHandler<CreateEmployeeCommond, int>
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

        public async Task<int> Handle(CreateEmployeeCommond request, CancellationToken cancellationToken)
        {
            var entity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Done = false
            };
            entity.DomainEvents.Add(new EmployeeCreatedEvent(entity));

            await _context.Employees.AddAsync(entity);

            await _context.SaveChangesAsync(cancellationToken);
            var entityParam = new EmployeeParams
            {
                EmployeeId = entity.Id,
                Username = request.Username,
                PasswordHash = _userService.CreatePasswordHash(request.password)
            };
            await _authContext.EmployeeParams.AddAsync(entityParam);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
