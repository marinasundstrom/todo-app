using MassTransit;
using MediatR;
using TodoApp.Contracts;

namespace TodoApp.Application.Features.Todos;

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

        await mediator.Send(new Application.Features.Todos.Commands.UpdateStatus(message.Id, (Application.Features.Todos.TodoStatusDto)message.Status));
    }
}