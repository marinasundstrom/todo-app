using MediatR;
using FluentValidation;

namespace TodoApp.Application.Todos.Commands;

public record DeleteTodo(int Id) : IRequest<Result>
{
    public class Validator : AbstractValidator<DeleteTodo>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<DeleteTodo, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(DeleteTodo request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            todoRepository.Remove(todo);

            todo.AddDomainEvent(new TodoDeleted(todo.Id));

            await todoRepository.SaveChangesAsync(cancellationToken);

            await todoNotificationService.Deleted(todo.Id);

            return Result.Success();
        }
    }
}
