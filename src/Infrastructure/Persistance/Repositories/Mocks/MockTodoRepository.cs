using System;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks;

public class MockTodoRepository : MockRepositoryBase<Todo, int>, ITodoRepository
{
    public MockTodoRepository(IDomainEventDispatcher domainEventDispatcher) : base(domainEventDispatcher)
    {
    }
}

