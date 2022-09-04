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

//if (args.Contains("--seed"))
//{
await Seed.EnsureSeedData(app.Services);
//    return;
//}

app.Run();

// INFO: Makes Program class visible to IntegrationTests.
public partial class Program { }