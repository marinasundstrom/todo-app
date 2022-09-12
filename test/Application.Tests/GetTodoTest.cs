using System.Threading.Tasks;
using NSubstitute;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Queries;
using TodoApp.Domain;
using TodoApp.Infrastructure.Persistance.Repositories.Mocks;
using Xunit;

namespace TodoApp.Application.Todos.Commands;

public class GetTodoTest
{
    [Fact]
    public async Task GetTodo_TodoNotFound()
    {
        // Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        // TODO: Fix with EF Core Sqlite provider
        var todoRepository = new MockTodoRepository(fakeDomainEventDispatcher);
        var commandHandler = new GetTodoById.Handler(todoRepository);

        int nonExistentTodoId = 9999;

        // Act

        var getTodoByIdCommand = new GetTodoById(nonExistentTodoId);

        var result = await commandHandler.Handle(getTodoByIdCommand, default);

        // Assert

        Assert.True(result.HasError(Errors.Todos.TodoNotFound));
    }
}
