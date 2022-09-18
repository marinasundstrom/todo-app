using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public sealed record DeleteTodo(int Id) : IRequest<Result>
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
