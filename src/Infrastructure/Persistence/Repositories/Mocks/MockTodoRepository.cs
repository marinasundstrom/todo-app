using System;
using TodoApp.Application.Specifications;

namespace TodoApp.Infrastructure.Persistence.Repositories.Mocks;

public sealed class MockTodoRepository : ITodoRepository
{
    private readonly MockUnitOfWork mockUnitOfWork;

    public MockTodoRepository(MockUnitOfWork mockUnitOfWork)
    {
        this.mockUnitOfWork = mockUnitOfWork;
    }

    public void Add(Todo item)
    {
        mockUnitOfWork.Items.Add(item);
        mockUnitOfWork.NewItems.Add(item);
    }

    public void Dispose()
    {
        foreach (var item in mockUnitOfWork.NewItems)
        {
            mockUnitOfWork.Items.Remove(item);
        }
    }

    public Task<Todo?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = mockUnitOfWork.Items
            .OfType<Todo>()
            .FirstOrDefault(x => x.Id.Equals(id));

        return Task.FromResult(item);
    }

    public IQueryable<Todo> GetAll()
    {
        return mockUnitOfWork.Items
            .OfType<Todo>()
            .AsQueryable();
    }

    public IQueryable<Todo> GetAll(ISpecification<Todo> specification)
    {
        return mockUnitOfWork.Items
            .OfType<Todo>()
            .AsQueryable()
            .Where(specification.Criteria);
    }

    public void Remove(Todo item)
    {
        mockUnitOfWork.Items.Remove(item);
    }
}

