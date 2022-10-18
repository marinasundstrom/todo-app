using TodoApp.Application.Common;
using TodoApp.Application.Services;

namespace TodoApp.Application.Todos.EventHandlers;

public sealed class TodoAssignedUserEventHandler : IDomainEventHandler<TodoAssignedUserUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly IEmailService emailService;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoAssignedUserEventHandler(ITodoRepository todoRepository, IEmailService emailService, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.emailService = emailService;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        if (todo.AssignedToId is not null && todo.LastModifiedById != todo.AssignedToId)
        {
            await emailService.SendEmail(
                todo.AssignedTo!.Email,
                $"You were assigned to \"{todo.Title}\" [{todo.Id}].",
                $"{todo.LastModifiedBy!.Name} assigned {todo.AssignedTo.Name} to \"{todo.Title}\" [{todo.Id}].");
        }
    }
}
