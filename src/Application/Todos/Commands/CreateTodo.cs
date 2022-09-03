using FluentValidation;
using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Commands;

public record CreateTodo(string Title, string? Description, TodoStatusDto Status) : IRequest<Result<TodoDto>>
{
    public class Validator : AbstractValidator<CreateTodo>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Description).MaximumLength(240);
        }
    }

    public class Handler : IRequestHandler<CreateTodo, Result<TodoDto>>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result<TodoDto>> Handle(CreateTodo request, CancellationToken cancellationToken)
        {
            var todo = new Todo(request.Title, request.Description, (Domain.Enums.TodoStatus)request.Status);

            todoRepository.Add(todo);

            await todoRepository.SaveChangesAsync(cancellationToken);

            await todoNotificationService.Created(todo.Id);

            return Result.Success(todo.ToDto());
        }
    }
}
