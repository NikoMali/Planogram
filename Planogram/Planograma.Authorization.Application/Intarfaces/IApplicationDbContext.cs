using Microsoft.EntityFrameworkCore;
using Planograma.Authorization.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.Authorization.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<EmployeeParams> EmployeeParams { get; set; }

        public abstract void SaveChanges();
    }
}
