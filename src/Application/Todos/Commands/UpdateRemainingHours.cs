using FluentValidation;
using MediatR;

namespace TodoApp.Application.Todos.Commands;

public record UpdateRemainingHours(int Id, double? Hours) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateRemainingHours>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateRemainingHours, Result>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result> Handle(UpdateRemainingHours request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateRemainingHours(request.Hours);
            await todoRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
