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

namespace Planograma.EmplUser.Application.Employees.Queries.GetEmployee
{
    public class GetEmployeeQuery : IRequest<EmployeeResponse>
    {
        public int Id { get; set; }
    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {

            return await _context.Employees
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

    }
}
