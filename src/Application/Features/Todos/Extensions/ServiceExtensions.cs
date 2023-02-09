using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Application.Features.Todos;

public static class ServiceExtensions
{
    public static IServiceCollection AddTodoControllers(this IServiceCollection services)
    {
        /*
        var assembly = typeof(TodosController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);
        */

        return services;
    }
}
