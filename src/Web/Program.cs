using System.Reflection.Metadata;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors.Security;
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

builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

builder.Services.AddVersionedApiExplorer(option =>
        {
            option.GroupNameFormat = "VVV";
            option.SubstituteApiVersionInUrl = true;
        });


// Register the Swagger services

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
var provider = builder.Services
    .BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
{
    builder.Services.AddOpenApiDocument(config =>
    {
        config.DocumentName = $"v{description.ApiVersion}";
        config.PostProcess = document =>
        {
            document.Info.Title = "Todo API";
            document.Info.Version = $"v{description.ApiVersion.ToString()}";
        };
        config.ApiGroupNames = new[] { description.ApiVersion.ToString() };

        config.AddSecurity("JWT", new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
        });

        /*
        config.AddSecurity("ApiKey", new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: {your API key}."
        });
        */

        config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ApiKey"));
    });
}

builder.Services.AddSignalR();

builder.Services.AddUniverse(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddMassTransitForApp();

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

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

    var dbProviderName = context.Database.ProviderName;

    if (dbProviderName!.Contains("SqlServer"))
    {
        //await context.Database.EnsureDeletedAsync();
        //await context.Database.EnsureCreatedAsync();

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

        return;
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