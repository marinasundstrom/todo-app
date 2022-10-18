using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Specifications;

namespace TodoApp.Infrastructure.Persistence.Repositories;

public sealed class TodoRepository : ITodoRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Todo> dbSet;

    public TodoRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Todo>();
    }

    public IQueryable<Todo> GetAll()
    {
        //return dbSet.Where(new TodosWithStatus(TodoStatus.Completed).Or(new TodosWithStatus(TodoStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Todo?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IQueryable<Todo> GetAll(ISpecification<Todo> specification)
    {
        return dbSet
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Todo item)
    {
        dbSet.Add(item);
    }

    public void Remove(Todo item)
    {
        dbSet.Remove(item);
    }
}
