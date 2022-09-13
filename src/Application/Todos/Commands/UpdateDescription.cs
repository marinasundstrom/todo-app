using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateDescription(int Id, string? Description) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateDescription>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public class Handler : IRequestHandler<UpdateDescription, Result>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result> Handle(UpdateDescription request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateDescription(request.Description);
            await todoRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
