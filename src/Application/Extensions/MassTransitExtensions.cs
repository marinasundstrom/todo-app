using MassTransit;
using TodoApp.Application.Features.Todos;

namespace TodoApp.Application.Extensions;

public static class MassTransitExtensions
{
    public static IBusRegistrationConfigurator AddApplicationConsumers(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddTodoConsumers();

        return busRegistrationConfigurator;
    }
}
