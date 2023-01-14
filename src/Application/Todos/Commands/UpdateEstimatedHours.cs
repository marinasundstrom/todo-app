using FluentValidation;
using Mediator;

namespace TodoApp.Application.Todos.Commands;

public sealed record UpdateEstimatedHours(int Id, double? Hours) : IRequest<Result>
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

        public async ValueTask<Result> Handle(UpdateEstimatedHours request, CancellationToken cancellationToken)
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
