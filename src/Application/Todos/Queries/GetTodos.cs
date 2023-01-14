using Mediator;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Todos.Dtos;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Todos.Queries;

public record GetTodos(TodoStatusDto? Status, string? AssignedTo, int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<TodoDto>>
{
    public class Handler : IRequestHandler<GetTodos, ItemsResult<TodoDto>>
    {
        private readonly ITodoRepository todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async ValueTask<ItemsResult<TodoDto>> Handle(GetTodos request, CancellationToken cancellationToken)
        {
            var query = todoRepository.GetAll();

            if (request.Status is not null)
            {
                query = query.Where(x => x.Status == (TodoStatus)request.Status);
            }

            if (request.AssignedTo is not null)
            {
                query = query.Where(x => x.AssignedToId == request.AssignedTo);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var todos = await query
                .Include(i => i.AssignedTo)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<TodoDto>(todos.Select(x => x.ToDto()), totalCount);
        }
    }
}
