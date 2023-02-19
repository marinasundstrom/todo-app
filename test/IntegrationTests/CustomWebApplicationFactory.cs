using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using TodoApp;
using TodoApp.Features.Todos;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Interceptors;
using TodoApp.Application.Extensions;

namespace TodoApp.IntegrationTests;

public sealed class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>, IAsyncLifetime where TStartup : class
{
    static readonly TestcontainersContainer testContainer =
        new TestcontainersBuilder<TestcontainersContainer>()
        .WithImage("redis:latest")
        .WithPortBinding(6379, 6379)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
        .Build();

    public async Task InitializeAsync()
    {
        await testContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "Test", options => { });
        });

        builder.ConfigureServices(async services =>
        {
            var descriptor = services.Single(
        d => d.ServiceType ==
            typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlite($"Data Source=testdb.db");

                options.AddInterceptors(
                    sp.GetRequiredService<OutboxSaveChangesInterceptor>(),
                    sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

#if DEBUG
                options
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging();
#endif
            });

            services.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddTodoConsumers();
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                try
                {
                    await Utilities.InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await testContainer.StopAsync();
    }
}