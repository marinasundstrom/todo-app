using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Persistance;
using TodoApp.Presentation;
using TodoApp.Web;
using TodoApp.Web.Middleware;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:5021")
                          .AllowAnyHeader().AllowAnyMethod();
                      });
});

// Add services to the container.

// Register the Swagger services
builder.Services.AddOpenApiDocument(document =>
        {
            document.Title = "Todo API";
            document.Version = "v1";
        });

builder.Services.AddSignalR();

builder.Services.AddUniverse(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddMassTransitForApp();

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHubsForApp();

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    //await context.Database.EnsureDeletedAsync();
    //await context.Database.EnsureCreatedAsync();

    var dbProviderName = context.Database.ProviderName;

    if (dbProviderName!.Contains("SqlServer"))
    {
        try
        {
            await ApplyMigrations(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred when applying migrations to the " +
                "database. Error: {Message}", ex.Message);
        }
    }

    if (args.Contains("--seed"))
    {
        try
        {
            await Seed.SeedData(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the " +
                "database. Error: {Message}", ex.Message);
        }
    }
}

app.Run();

static async Task ApplyMigrations(ApplicationDbContext context)
{
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Count() > 0)
    {
        await context.Database.MigrateAsync();
    }
}

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }