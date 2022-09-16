using MediatR;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoDeletedEventHandler : INotificationHandler<DomainEventNotification<TodoDeleted>>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDeletedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(DomainEventNotification<TodoDeleted> notification, CancellationToken cancellationToken)
    {
        await todoNotificationService.Deleted(notification.DomainEvent.TodoId);
    }
}

