using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoEstimatedHoursUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoEstimatedHoursUpdated>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoEstimatedHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoEstimatedHoursUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.EstimatedHoursUpdated(todo.Id, todo.EstimatedHours);
    }
}
