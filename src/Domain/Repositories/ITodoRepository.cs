using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo, string>
{
    // NOTE: Just for DEMO. Belongs in UoW.
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}