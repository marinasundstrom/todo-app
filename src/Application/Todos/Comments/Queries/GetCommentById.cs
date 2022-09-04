using MediatR;
using TodoApp.Application.Todos.Dtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Application.Todos.Comments.Queries;

public record GetCommentById(string TodoId, string CommentId) : IRequest<Result<CommentDto>>
{
    public class Validator : AbstractValidator<GetCommentById>
    {
        public Validator()
        {
            RuleFor(x => x.TodoId).NotEmpty();

            RuleFor(x => x.CommentId).NotEmpty();
        }
    }
    
    public class Handler : IRequestHandler<GetCommentById, Result<CommentDto>>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<Result<CommentDto>> Handle(GetCommentById request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.TodoId, cancellationToken);

            if (todo is null)
            {
                return Result.Failure<CommentDto>(Errors.Todos.TodoNotFound);
            }

            var comment =  await todoRepository
                .GetCommentsForTodo(request.TodoId)
                .FirstAsync(c => c.Id == request.CommentId, cancellationToken);

            if (comment is null)
            {
                return Result.Failure<CommentDto>(Errors.Todos.CommentNotFound);
            }

            return Result.Success(comment.ToDto());
        }
    }
}