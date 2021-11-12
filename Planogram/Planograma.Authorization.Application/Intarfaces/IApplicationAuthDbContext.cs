using Microsoft.EntityFrameworkCore;
using Planograma.Authorization.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.Authorization.Application.Interfaces
{
    public interface IApplicationAuthDbContext
    {
        DbSet<EmployeeParams> EmployeeParams { get; set; }
        DbSet<EmployeeRole> EmployeeRoles { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<AuthenticationInfo> AuthenticationInfos { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
       
    }
}
