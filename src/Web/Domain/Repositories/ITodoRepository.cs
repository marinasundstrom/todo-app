using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo, TodoId>
{
}
