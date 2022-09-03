using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoDeletedEventHandler : INotificationHandler<DomainEventNotification<TodoDeleted>>
{
    private readonly ITodoRepository todoRepository;

    public TodoDeletedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public Task Handle(DomainEventNotification<TodoDeleted> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

