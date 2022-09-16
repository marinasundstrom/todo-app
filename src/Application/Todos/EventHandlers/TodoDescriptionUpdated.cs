using MediatR;
using TodoApp.Application.Services;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoDescriptionUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoDescriptionUpdated>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDescriptionUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoDescriptionUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.DescriptionUpdated(todo.Id, todo.Description);
    }
}

