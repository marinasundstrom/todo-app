using System;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks;

public class MockTodoRepository : MockRepositoryBase<Todo, string>, ITodoRepository
{
    public MockTodoRepository(IDomainEventDispatcher domainEventDispatcher) : base(domainEventDispatcher)
    {
    }
}

