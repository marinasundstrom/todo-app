using TodoApp.Application.Domain.ValueObjects;

namespace TodoApp.Application.Domain.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
}