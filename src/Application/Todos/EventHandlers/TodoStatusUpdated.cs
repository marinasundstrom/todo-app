using MediatR;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoStatusUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoStatusUpdated>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoStatusUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoStatusUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);
    }
}
