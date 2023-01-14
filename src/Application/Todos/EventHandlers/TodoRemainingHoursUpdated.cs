using Mediator;
using TodoApp.Application.Common;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoRemainingHoursUpdatedEventHandler : IDomainEventHandler<TodoRemainingHoursUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoRemainingHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async ValueTask Handle(TodoRemainingHoursUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.RemainingHoursUpdated(todo.Id, todo.RemainingHours);
    }
}