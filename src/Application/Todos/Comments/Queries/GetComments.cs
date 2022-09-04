using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Dtos;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Todos.Comments.Queries;

public record GetComments(string TodoId, int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<CommentDto>>
{
    public class Handler : IRequestHandler<GetComments, ItemsResult<CommentDto>>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<ItemsResult<CommentDto>> Handle(GetComments request, CancellationToken cancellationToken)
        {
            var query = todoRepository.GetCommentsForTodo(request.TodoId);

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else 
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var comments = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<CommentDto>(comments.Select(x => x.ToDto()), totalCount);
        }
    }
}
