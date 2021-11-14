using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Application.Services;
using Planograma.Authorization.Domain.Entities;
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
    public class UpdateEmployeeCommand : IRequest<UpdateEmployeeCommand>, IMapFrom<Employee>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string MobileNumber { get; set; }
        public string password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateEmployeeCommand, Employee>();   
            profile.CreateMap<Employee, UpdateEmployeeCommand>();

            profile.CreateMap<UpdateEmployeeCommand, EmployeeParams>();
            profile.CreateMap<EmployeeParams, UpdateEmployeeCommand>();
        }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IApplicationAuthDbContext _authContext;



        public UpdateEmployeeCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            IUserService userService,
            IApplicationAuthDbContext authContext

            )
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _authContext = authContext;


        }

        public async Task<UpdateEmployeeCommand> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employeeMapper = _mapper.Map<Employee>(request);
            var entity = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            //entity.FirstName = request.FirstName;
            //entity.LastName = request.LastName;
            var entityParam =await _authContext.EmployeeParams.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == request.Id);
            entityParam.EmployeeId = request.Id;
            entityParam.PasswordHash = _userService.CreatePasswordHash(request.password);





            _context.Employees.Update(employeeMapper);
            _authContext.EmployeeParams.Update(entityParam);
            await _context.SaveChangesAsync(cancellationToken);

            //await _authContext.SaveChangesAsync(cancellationToken);

            return request;
        }
    }
}
