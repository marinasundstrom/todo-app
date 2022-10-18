using TodoApp.Application.Common;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoAssignedUserEventHandler : IDomainEventHandler<TodoAssignedUserUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoAssignedUserEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        //await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);
    }
}
