using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Persistence.Interceptors;
using TodoApp.Infrastructure.Persistence.Repositories;

namespace TodoApp.Infrastructure.Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            const string ConnectionStringKey = "mssql";

            var connectionString = Infrastructure.ConfigurationExtensions.GetConnectionString(configuration, ConnectionStringKey, "TodoApp")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString!, o => o.EnableRetryOnFailure());
#if DEBUG
                options
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();
#endif
            });

            //services.AddScoped<IApplicationContext>(sp => sp.GetRequiredService<ApplicationContext>());

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            RegisterRepositories(services);

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            // TODO: Automate this

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();

            services.AddScoped<ITodoRepository, TodoRepository>();
        }
    }
}
