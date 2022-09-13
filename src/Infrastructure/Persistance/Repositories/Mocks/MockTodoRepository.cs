using System;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks;

public sealed class MockTodoRepository : MockRepositoryBase<Todo, int>, ITodoRepository
{
    public MockTodoRepository(MockUnitOfWork mockUnitOfWork) : base(mockUnitOfWork)
    {
    }
}

