using FluentValidation;
using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Commands;

public record UpdateStatus(int Id, TodoStatusDto Status) : IRequest<Result>
{
    public class Validator : AbstractValidator<UpdateStatus>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<UpdateStatus, Result>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todo.UpdateStatus((Domain.Enums.TodoStatus)request.Status);
            await todoRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
