using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoStatusUpdatedEventHandler : IDomainEventHandler<TodoStatusUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IEmailService emailService;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoStatusUpdatedEventHandler(ITodoRepository todoRepository, ICurrentUserService currentUserService, IEmailService emailService, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.currentUserService = currentUserService;
        this.emailService = emailService;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoStatusUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);

        if (todo.AssignedToId is not null && todo.LastModifiedById != todo.AssignedToId)
        {
            await emailService.SendEmail(todo.AssignedTo!.Email,
                $"Status of \"{todo.Title}\" [{todo.Id}] changed to {notification.NewStatus}.",
                $"{todo.LastModifiedBy!.Name} changed status of \"{todo.Title}\" [{todo.Id}] from {notification.OldStatus} to {notification.NewStatus}.");
        }
    }
}
