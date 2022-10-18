using TodoApp.Domain.Entities;
using TodoApp.Domain.Specifications;

namespace TodoApp.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo>
{
    IQueryable<Todo> GetAll();
    IQueryable<Todo> GetAll(ISpecification<Todo> specification);
    Task<Todo?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Todo item);
    void Remove(Todo item);
}
