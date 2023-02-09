using MassTransit;
using MediatR;
using TodoApp.Contracts;

namespace TodoApp.Features.Todos;

public sealed class UpdateStatusConsumer : IConsumer<UpdateStatus>
{
    private readonly IMediator mediator;

    public UpdateStatusConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<UpdateStatus> context)
    {
        var message = context.Message;

        await mediator.Send(new Features.Todos.Commands.UpdateStatus(message.Id, (Features.Todos.TodoStatusDto)message.Status));
    }
}