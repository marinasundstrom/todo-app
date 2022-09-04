using MediatR;
using FluentValidation;

namespace TodoApp.Application.Todos.Comments.Commands;

public record DeleteComment(string TodoId, string CommentId) : IRequest<Result>
{
    public class Validator : AbstractValidator<DeleteComment>
    {
        public Validator()
        {
            RuleFor(x => x.TodoId).NotEmpty();

            RuleFor(x => x.CommentId).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<DeleteComment, Result>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result> Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.TodoId, cancellationToken);

            if (todo is null)
            {
                return Result.Failure(Errors.Todos.TodoNotFound);
            }

            var comment = todo.Comments.First(c => c.Id == request.CommentId);

            if (comment is null)
            {
                return Result.Failure(Errors.Todos.CommentNotFound);
            }

            todo.DeleteComment(comment);

            //todo.AddDomainEvent(new TodoDeleted(todo.Id));

            await todoRepository.SaveChangesAsync(cancellationToken);

            //await todoNotificationService.Deleted(todo.Id);

            return Result.Success();
        }
    }
}
