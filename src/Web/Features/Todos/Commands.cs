using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Features.Todos.Commands;

public sealed record CreateTodo(string Title, string? Description, TodoStatusDto Status, string? AssignedTo, double? EstimatedHours, double? RemainingHours) : IRequest<Result<TodoDto>>
{
    public sealed class Validator : AbstractValidator<CreateTodo>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<CreateTodo, Result<TodoDto>>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<TodoDto>> Handle(CreateTodo request, CancellationToken cancellationToken)
        {
            var todo = new Todo(request.Title, request.Description, (Domain.Enums.TodoStatus)request.Status);

            todo.UpdateEstimatedHours(request.EstimatedHours);
            todo.UpdateRemainingHours(request.RemainingHours);

            todoRepository.Add(todo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.AssignedTo is not null)
            {
                todo.UpdateAssignedTo(request.AssignedTo);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                todo.ClearDomainEvents();
            }

            await domainEventDispatcher.Dispatch(new TodoCreated(todo.Id), cancellationToken);

            todo = await todoRepository.GetAll()
                .OrderBy(i => i.Id)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .LastAsync(cancellationToken);

            return Result.Success(todo!.ToDto());
        }
    }
}

public sealed record DeleteTodo(TodoId Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteTodo>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteTodo, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTodo request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todoRepository.Remove(todo);

            todo.AddDomainEvent(new TodoDeleted(todo.Id, todo.Title));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateAssignedUser(TodoId Id, string? UserId) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateAssignedUser>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateAssignedUser, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAssignedUser request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            if (request.UserId is not null)
            {
                var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

                if (user is null)
                {
                    return Result.Failure(Errors.Users.UserNotFound);
                }
            }

            todo.UpdateAssignedTo(request.UserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateDescription(TodoId Id, string? Description) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateDescription>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateDescription, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateDescription request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateDescription(request.Description);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateEstimatedHours(TodoId Id, double? Hours) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateEstimatedHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateEstimatedHours, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateEstimatedHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateEstimatedHours(request.Hours);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateRemainingHours(TodoId Id, double? Hours) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateRemainingHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateRemainingHours, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateRemainingHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateRemainingHours(request.Hours);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateStatus(TodoId Id, TodoStatusDto Status) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateStatus>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateStatus, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateStatus((TodoStatus)request.Status);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

public sealed record UpdateTitle(TodoId Id, string Title) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<UpdateTitle>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);
        }
    }

    public sealed class Handler : IRequestHandler<UpdateTitle, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
        {
            this.todoRepository = todoRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateTitle(request.Title);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
