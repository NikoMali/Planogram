using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Planograma.Authorization.Application.Interfaces;
using Planograma.Authorization.Infrastructure.Contexts;

namespace Planograma.Authorization.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           
            var connectionString = "server=localhost; database=Planograma; uid=root; pwd=Gaixsna122;";
            services.AddDbContext<ApplicationAuthDbContext>(options =>
                    options.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString),
                        b => b.MigrationsAssembly(typeof(ApplicationAuthDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationAuthDbContext>());

            


            return services;
        }
    }
}