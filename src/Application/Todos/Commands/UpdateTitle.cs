using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateTitle(int Id, string Title) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateTitle>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);
        }
    }

    public class Handler : IRequestHandler<UpdateTitle, Result>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateTitle(request.Title);
            await todoRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
