using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Services;
using TodoApp.Presentation.Controllers;
using TodoApp.Presentation.Hubs;

namespace TodoApp.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<ITodoNotificationService, TodoNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(TodosController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
