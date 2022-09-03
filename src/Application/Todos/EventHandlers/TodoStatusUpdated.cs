using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoStatusUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoStatusUpdated>>
{
    private readonly ITodoRepository todoRepository;

    public TodoStatusUpdatedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task Handle(DomainEventNotification<TodoStatusUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if(todo is not null) 
        {

        }
    }
}