using TodoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoApp.Infrastructure.Persistance;

public class Seed
{
    public static async Task EnsureSeedData(IServiceProvider services)
    {
        using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Seed>>();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            //await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if(pendingMigrations.Count() > 0) 
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}