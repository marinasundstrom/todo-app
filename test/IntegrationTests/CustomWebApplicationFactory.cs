using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistance;

namespace TodoApp.IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
        d => d.ServiceType ==
            typeof(DbContextOptions<ApplicationContext>));

            services.Remove(descriptor);

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlite("Data Source=testdb.db");
            });

            services.AddMassTransitTestHarness(cfg =>
            {
                //cfg.AddConsumer<SubmitOrderConsumer>();
            });
        });
    }
}