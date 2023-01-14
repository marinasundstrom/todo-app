using Mediator;
using TodoApp.Application.Common;
using TodoApp.Application.Services;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoUpdatedEventHandler : IDomainEventHandler<TodoUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async ValueTask Handle(TodoUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Updated(todo.Id, todo.Title);
    }
}
