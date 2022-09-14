using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Specifications;

namespace TodoApp.Infrastructure.Persistence.Repositories;

public class RepositoryBase<T, TKey> : IRepository<T, TKey>
    where T : Entity, IAggregateRoot<TKey>
    where TKey : notnull
{
    protected readonly ApplicationDbContext context;
    protected readonly DbSet<T> dbSet;

    public RepositoryBase(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<T>();
    }

    public virtual void Add(T item)
    {
        dbSet.Add(item);
    }

    public virtual async Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public virtual IQueryable<T> GetAll()
    {
        return dbSet.AsQueryable();
    }

    public virtual IQueryable<T> GetAll(ISpecification<T> specification)
    {
        return dbSet.Where(specification.Criteria);
    }

    public virtual void Remove(T item)
    {
        dbSet.Remove(item);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}