using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Domain.Entities;
using Planograma.EmplUser.Domain.Common;
using Planograma.EmplUser.Domain.Entities;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Planograma.Authorization.Infrastructure.Contexts
{
    public class ApplicationAuthDbContext : DbContext, IApplicationDbContext
    {
        
        


        public ApplicationAuthDbContext(
            DbContextOptions<ApplicationAuthDbContext> options
            ) : base(options)
        {
         
        }
       

        public DbSet<EmployeeParams> EmployeeParams { get; set; }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public void SaveChanges()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            base.SaveChanges();
        }


    }
}
