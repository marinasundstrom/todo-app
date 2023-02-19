using MassTransit;
using TodoApp.Features.Todos;

namespace TodoApp.Extensions;

public static class MassTransitExtensions
{
    public static IBusRegistrationConfigurator AddApplicationConsumers(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddTodoConsumers();

        return busRegistrationConfigurator;
    }
}
