using MediatR;
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

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateEmployeeCommond, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTodoItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateEmployeeCommond request, CancellationToken cancellationToken)
        {
            var entity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.password,
                Done = false
            };

            entity.DomainEvents.Add(new EmployeeCreatedEvent(entity));

            _context.Employees.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
