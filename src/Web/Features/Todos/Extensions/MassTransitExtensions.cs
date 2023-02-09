using MassTransit;

namespace TodoApp.Features.Todos;

public static class MassTransitExtensions
{
    public static IBusRegistrationConfigurator AddTodoConsumers(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        busRegistrationConfigurator.AddConsumer<UpdateStatusConsumer>();

        return busRegistrationConfigurator;
    }
}
