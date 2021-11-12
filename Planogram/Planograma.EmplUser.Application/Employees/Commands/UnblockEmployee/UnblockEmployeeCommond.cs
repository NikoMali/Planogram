using MediatR;
using Microsoft.EntityFrameworkCore;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Application.Services;
using Planograma.Authorization.Domain.Entities;
using Planograma.EmplUser.Application.Interfaces;
using Planograma.EmplUser.Application.Models;
using Planograma.EmplUser.Domain.Entities;
using Planograma.EmplUser.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.UnblockEmployee
{
    public class UnblockEmployeeCommond : IRequest<Result>
    {
        public int EmployeeId { get; set; }
   
    }

    public class UnblockEmployeeCommondHandler : IRequestHandler<UnblockEmployeeCommond, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationAuthDbContext _authContext;
        private readonly IUserService _userService;

        public UnblockEmployeeCommondHandler(
            IApplicationDbContext context,
            IApplicationAuthDbContext authContext,
            IUserService userService
            )
        {
            _context = context;
            _authContext = authContext;
            _userService = userService;
        }

        public async Task<Result> Handle(UnblockEmployeeCommond request, CancellationToken cancellationToken)
        {
            var employeeAuth =await _authContext.AuthenticationInfos
                .Where(x => x.EmployeeId == request.EmployeeId && x.IsDelete == false)
                .ToListAsync();

            for (int i = 0; i < employeeAuth.Count; i++)
            {
                employeeAuth[i].IsDelete = true;
            }
            _authContext.AuthenticationInfos.UpdateRange(employeeAuth);
            await _authContext.SaveChangesAsync();

            return new Result(true);
        }
    }
}
