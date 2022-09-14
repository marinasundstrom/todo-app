using System;

namespace TodoApp.Infrastructure.Persistence.Repositories.Mocks;

public sealed class MockTodoRepository : MockRepositoryBase<Todo, int>, ITodoRepository
{
    public MockTodoRepository(MockUnitOfWork mockUnitOfWork) : base(mockUnitOfWork)
    {
    }
}

