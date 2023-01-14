using FluentValidation;
using Mediator;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Commands;

public sealed record UpdateStatus(int Id, TodoStatusDto Status) : IRequest<Result>
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

        public async ValueTask<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
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
