using MediatR;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoTitleUpdatedEventHandler : INotificationHandler<DomainEventNotification<TodoTitleUpdated>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoTitleUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoTitleUpdated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.TitleUpdated(todo.Id, todo.Title);
    }
}
