using AutoMapper;
using MediatR;
using Planograma.EmplUser.Application.Helpers.Exceptions;
using Planograma.EmplUser.Application.Interfaces;
using Planograma.EmplUser.Application.Mappings;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest, IMapFrom<Employee>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateEmployeeCommand, Employee>();   
            profile.CreateMap<Employee, UpdateEmployeeCommand>();
        }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employeeMapper = _mapper.Map<Employee>(request);
            var entity = await _context.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            
            
            

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
