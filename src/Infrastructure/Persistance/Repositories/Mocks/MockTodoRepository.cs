using System;

namespace TodoApp.Infrastructure.Persistance.Repositories.Mocks;

public class MockTodoRepository : MockRepositoryBase<Todo, string>, ITodoRepository
{
    public MockTodoRepository(IDomainEventDispatcher domainEventDispatcher) : base(domainEventDispatcher)
    {
    }

    public IQueryable<Comment> GetCommentsForTodo(string todoId)
    {
        var item = items.First(x => x.Id == todoId);
        return item.Comments.AsQueryable();
    }
}

