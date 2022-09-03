using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoUpdated>>
{
    private readonly ITodoRepository todoRepository;

    public TodoUpdatedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task Handle(DomainEventNotification<TodoUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if(todo is not null) 
        {

        }
    }
}
