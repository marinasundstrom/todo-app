using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Repositories;

public interface IRepository<T>
    where T : IAggregateRoot
{

}
