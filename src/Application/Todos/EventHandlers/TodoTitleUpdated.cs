using Mediator;
using TodoApp.Application.Common;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoTitleUpdatedEventHandler : IDomainEventHandler<TodoTitleUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoTitleUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async ValueTask Handle(TodoTitleUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.TitleUpdated(todo.Id, todo.Title);
    }
}
