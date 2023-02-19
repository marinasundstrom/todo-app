using TodoApp.Application.Domain.ValueObjects;

namespace TodoApp.Application.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo, TodoId>
{
}
