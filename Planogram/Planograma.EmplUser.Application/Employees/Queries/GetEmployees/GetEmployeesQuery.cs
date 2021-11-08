using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Planograma.EmplUser.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Queries.GetEmployees
{
    public class GetEmployeesQuery: IRequest<List<EmployeesResponse>>
    {
    }

    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery,List<EmployeesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeesResponse>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
           
            return await _context.Employees
                .OrderBy(x => x.Id)
                .ProjectTo<EmployeesResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

    }
}
