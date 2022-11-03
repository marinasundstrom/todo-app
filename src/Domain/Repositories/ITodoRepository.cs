using TodoApp.Domain.Entities;
using TodoApp.Domain.Specifications;

namespace TodoApp.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo, int>
{
}
