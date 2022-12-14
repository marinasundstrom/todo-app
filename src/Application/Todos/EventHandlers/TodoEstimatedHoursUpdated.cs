using MediatR;
using TodoApp.Application.Common;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoEstimatedHoursUpdatedEventHandler : IDomainEventHandler<TodoEstimatedHoursUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoEstimatedHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoEstimatedHoursUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.EstimatedHoursUpdated(todo.Id, todo.EstimatedHours);
    }
}
