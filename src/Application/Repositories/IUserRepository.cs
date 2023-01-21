using TodoApp.Domain.Entities;
using TodoApp.Domain.Specifications;

namespace TodoApp.Domain.Repositories;

public interface IUserRepository : IRepository<User, string>
{
}