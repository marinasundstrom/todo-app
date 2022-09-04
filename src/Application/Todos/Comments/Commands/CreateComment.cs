using FluentValidation;
using MediatR;
using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Application.Todos.Comments.Commands;

public record CreateComment(string TodoId, string Text) : IRequest<Result<CommentDto>>
{
    public class Validator : AbstractValidator<CreateComment>
    {
        public Validator()
        {
            RuleFor(x => x.Text).MaximumLength(240);
        }
    }

    public class Handler : IRequestHandler<CreateComment, Result<CommentDto>>
    {
        private readonly ITodoRepository todoRepository;
        private readonly ITodoNotificationService todoNotificationService;

        public Handler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
        {
            this.todoRepository = todoRepository;
            this.todoNotificationService = todoNotificationService;
        }

        public async Task<Result<CommentDto>> Handle(CreateComment request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.TodoId, cancellationToken);

            if (todo is null)
            {
                return Result.Failure<CommentDto>(Errors.Todos.TodoNotFound);
            }

            var comment = new Comment(request.Text);

            todo.AddComment(comment);

            await todoRepository.SaveChangesAsync(cancellationToken);

            //await todoNotificationService.Created(todo.Id);

            return Result.Success(comment.ToDto());
        }
    }
}
