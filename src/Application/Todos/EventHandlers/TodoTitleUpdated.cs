using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoTitleUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoTitleUpdated>>
{
    private readonly ITodoRepository todoRepository;

    public TodoTitleUpdatedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task Handle(DomainEventNotification<TodoTitleUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is not null)
        {

        }
    }
}
