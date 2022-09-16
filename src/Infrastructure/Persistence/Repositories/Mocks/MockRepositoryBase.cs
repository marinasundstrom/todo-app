using TodoApp.Domain.Specifications;

namespace TodoApp.Infrastructure.Persistence.Repositories.Mocks;

public class MockRepositoryBase<T, TKey> : IRepository<T, TKey>, IDisposable
    where T : Entity, IAggregateRoot<TKey>
    where TKey : notnull
{
    private readonly MockUnitOfWork mockUnitOfWork;

    public MockRepositoryBase(MockUnitOfWork mockUnitOfWork)
    {
        this.mockUnitOfWork = mockUnitOfWork;
    }

    public virtual void Add(T item)
    {
        mockUnitOfWork.Items.Add(item);
        mockUnitOfWork.NewItems.Add(item);
    }

    public virtual void Dispose()
    {
        foreach (var item in mockUnitOfWork.NewItems)
        {
            mockUnitOfWork.Items.Remove(item);
        }
    }

    public virtual Task<T?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var item = mockUnitOfWork.Items
            .OfType<T>()
            .FirstOrDefault(x => x.Id.Equals(id));

        return Task.FromResult(item);
    }

    public virtual IQueryable<T> GetAll()
    {
        return mockUnitOfWork.Items
            .OfType<T>()
            .AsQueryable();
    }

    public virtual IQueryable<T> GetAll(ISpecification<T> specification)
    {
        return mockUnitOfWork.Items
            .OfType<T>()
            .AsQueryable()
            .Where(specification.Criteria);
    }

    public virtual void Remove(T item)
    {
        mockUnitOfWork.Items.Remove(item);
    }
}