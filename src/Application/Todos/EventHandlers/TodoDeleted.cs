using Mediator;
using TodoApp.Application.Common;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoDeletedEventHandler : IDomainEventHandler<TodoDeleted>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDeletedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async ValueTask Handle(TodoDeleted notification, CancellationToken cancellationToken)
    {
        await todoNotificationService.Deleted(notification.TodoId, notification.Title);
    }
}

