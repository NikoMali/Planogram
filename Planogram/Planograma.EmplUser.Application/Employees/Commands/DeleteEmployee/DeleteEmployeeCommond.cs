using MediatR;
using Planograma.EmplUser.Application.Helpers.Exceptions;
using Planograma.EmplUser.Application.Interfaces;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommond : IRequest
    {
        public int Id { get; set; }
    
    }

    public class DeleteEmployeeCommonddHandler : IRequestHandler<DeleteEmployeeCommond>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEmployeeCommonddHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommond request, CancellationToken cancellationToken)
        {
            var entity = await _context.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            _context.Employees.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
