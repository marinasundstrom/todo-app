using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoDescriptionUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoDescriptionUpdated>>
{
    private readonly ITodoRepository todoRepository;

    public TodoDescriptionUpdatedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task Handle(DomainEventNotification<TodoDescriptionUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is not null)
        {

        }
    }
}

