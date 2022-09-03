using TodoApp.Infrastructure.Persistance;
using TodoApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistance(configuration);

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDateTime, DateTimeService>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            return services;
        }
    }
}
