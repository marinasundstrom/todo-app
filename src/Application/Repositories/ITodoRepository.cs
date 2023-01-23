using TodoApp.Application.Entities;
using TodoApp.Application.Specifications;
using TodoApp.Application.ValueObjects;

namespace TodoApp.Application.Repositories;

public interface ITodoRepository : IRepository<Todo, TodoId>
{
}
