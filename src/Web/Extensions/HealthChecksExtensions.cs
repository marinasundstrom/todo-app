using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Web.Extensions;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}
