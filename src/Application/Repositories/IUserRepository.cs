using TodoApp.Application.ValueObjects;

namespace TodoApp.Application.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
}