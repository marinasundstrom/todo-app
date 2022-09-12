using FluentValidation;
using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Queries;

public record GetTodoById(int Id) : IRequest<Result<TodoDto>>
{
    public class Validator : AbstractValidator<GetTodoById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetTodoById, Result<TodoDto>>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result<TodoDto>> Handle(GetTodoById request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure<TodoDto>(Errors.Todos.TodoNotFound);
            }

            return Result.Success(todo.ToDto());
        }
    }
}