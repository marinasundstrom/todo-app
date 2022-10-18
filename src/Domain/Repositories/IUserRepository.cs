using TodoApp.Domain.Entities;
using TodoApp.Domain.Specifications;

namespace TodoApp.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    IQueryable<User> GetAll();
    IQueryable<User> GetAll(ISpecification<User> specification);
    Task<User?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    void Add(User user);
    void Remove(User user);
}