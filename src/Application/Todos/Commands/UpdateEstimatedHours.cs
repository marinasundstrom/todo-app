using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateEstimatedHours(int Id, double? Hours) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateEstimatedHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateEstimatedHours, Result>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result> Handle(UpdateEstimatedHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateEstimatedHours(request.Hours);
            await todoRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
