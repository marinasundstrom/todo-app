using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using TodoApp.Application.Specifications;
using TodoApp.Application.ValueObjects;

namespace TodoApp.Infrastructure.Persistence.Repositories;

public sealed class CachedTodoRepository : ITodoRepository
{
    private readonly ITodoRepository decorated;
    private readonly IMemoryCache memoryCache;

    public CachedTodoRepository(ITodoRepository decorated, IMemoryCache memoryCache)
    {
        this.decorated = decorated;
        this.memoryCache = memoryCache;
    }

    public void Add(Todo item)
    {
        decorated.Add(item);
    }

    public async Task<Todo?> FindByIdAsync(TodoId id, CancellationToken cancellationToken = default)
    {
        string key = $"todo-{id}";

        return await memoryCache.GetOrCreateAsync<Todo?>(key, async options =>
        {
            options.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(2);

            return await decorated.FindByIdAsync(id, cancellationToken);
        });
    }

    public IQueryable<Todo> GetAll()
    {
        return decorated.GetAll();
    }

    public IQueryable<Todo> GetAll(ISpecification<Todo> specification)
    {
        return decorated.GetAll(specification);
    }

    public void Remove(Todo item)
    {
        decorated.Remove(item);
    }
}
