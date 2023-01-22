using TodoApp.Application.Entities;
using TodoApp.Application.Specifications;

namespace TodoApp.Application.Repositories;

public interface IUserRepository : IRepository<User, string>
{
}