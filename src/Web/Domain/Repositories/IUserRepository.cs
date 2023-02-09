using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
}