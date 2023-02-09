using MassTransit;
using TodoApp.Features.Todos;

namespace TodoApp.Web.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            //x.AddConsumers(typeof(Program).Assembly);

            x.AddTodoConsumers();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
