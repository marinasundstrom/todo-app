using TodoApp.Domain.Entities;
using TodoApp.Domain.Specifications;

namespace TodoApp.Domain.Repositories;

public interface IRepository<T, TKey>
    where T : IAggregateRoot<TKey>
    where TKey : notnull
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(ISpecification<T> specification);
    Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);
    void Add(T item);
    void Remove(T item);
}
