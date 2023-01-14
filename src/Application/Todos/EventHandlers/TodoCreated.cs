using System;
using Mediator;
using TodoApp.Application.Common;
using TodoApp.Application.Services;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoCreatedEventHandler : IDomainEventHandler<TodoCreated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoCreatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async ValueTask Handle(TodoCreated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Created(todo.Id, todo.Title);
    }
}

