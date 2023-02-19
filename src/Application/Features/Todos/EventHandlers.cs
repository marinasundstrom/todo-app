using TodoApp.Common;

namespace TodoApp.Features.Todos.EventHandlers;

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

public sealed class TodoCreatedEventHandler : IDomainEventHandler<TodoCreated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoCreatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoCreated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Created(todo.Id, todo.Title);
    }
}

public sealed class TodoDeletedEventHandler : IDomainEventHandler<TodoDeleted>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDeletedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoDeleted notification, CancellationToken cancellationToken)
    {
        await todoNotificationService.Deleted(notification.TodoId, notification.Title);
    }
}

public sealed class TodoDescriptionUpdatedEventHandler : IDomainEventHandler<TodoDescriptionUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDescriptionUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoDescriptionUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.DescriptionUpdated(todo.Id, todo.Description);
    }
}

public sealed class TodoEstimatedHoursUpdatedEventHandler : IDomainEventHandler<TodoEstimatedHoursUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoEstimatedHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoEstimatedHoursUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.EstimatedHoursUpdated(todo.Id, todo.EstimatedHours);
    }
}

public sealed class TodoRemainingHoursUpdatedEventHandler : IDomainEventHandler<TodoRemainingHoursUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoRemainingHoursUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoRemainingHoursUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.RemainingHoursUpdated(todo.Id, todo.RemainingHours);
    }
}

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

public sealed class TodoTitleUpdatedEventHandler : IDomainEventHandler<TodoTitleUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoTitleUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoTitleUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.TitleUpdated(todo.Id, todo.Title);
    }
}

public sealed class TodoUpdatedEventHandler : IDomainEventHandler<TodoUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Updated(todo.Id, todo.Title);
    }
}
