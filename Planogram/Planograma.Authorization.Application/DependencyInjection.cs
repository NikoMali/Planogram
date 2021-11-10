using Microsoft.Extensions.DependencyInjection;
using Planograma.Authorization.Application.Authorization;
using Planograma.Authorization.Application.Services;

namespace Planograma.Authorization.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthApplication(this IServiceCollection services)
        {
           

            // configure DI for application services
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, AuthService>();

            return services;
        }
    }
}
