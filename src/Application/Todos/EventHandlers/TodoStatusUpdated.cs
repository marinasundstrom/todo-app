using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoStatusUpdatedEventHandler : IDomainEventHandler<TodoStatusUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoStatusUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoStatusUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);
    }
}
