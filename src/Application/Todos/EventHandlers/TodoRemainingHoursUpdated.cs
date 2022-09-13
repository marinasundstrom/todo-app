using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoRemainingHoursUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoRemainingHoursUpdated>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoRemainingHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoRemainingHoursUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.RemainingHoursUpdated(todo.Id, todo.RemainingHours);
    }
}