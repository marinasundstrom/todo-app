using System;
using MediatR;

namespace TodoApp.Application.Todos.EventHandlers;

public class TodoCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoCreated>>
{
    private readonly ITodoRepository todoRepository;

    public TodoCreatedEventHandler(ITodoRepository todoRepository)
    {
        this.todoRepository = todoRepository;
    }

    public async Task Handle(DomainEventNotification<TodoCreated> notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.DomainEvent.TodoId, cancellationToken);
    }
}

