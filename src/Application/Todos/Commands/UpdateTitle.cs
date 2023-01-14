using FluentValidation;
using Mediator;

namespace TodoApp.Application.Todos.Commands;

public sealed record UpdateTitle(int Id, string Title) : IRequest<Result>
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

        public async ValueTask<Result> Handle(UpdateTitle request, CancellationToken cancellationToken)
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
