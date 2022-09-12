using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using TodoApp.Application.Services;
using TodoApp.Domain.Events;
using TodoApp.Infrastructure.Persistance.Repositories.Mocks;
using Xunit;

namespace TodoApp.Application.Todos.Commands;

public class CreateTodoTest
{
    [Fact]
    public async Task CreateTodo_TodoCreated()
    {
        // Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        var fakeTodoNotificationService = Substitute.For<ITodoNotificationService>();

        // TODO: Fix with EF Core Sqlite provider
        var todoRepository = new MockTodoRepository(fakeDomainEventDispatcher);
        var commandHandler = new CreateTodo.Handler(todoRepository, fakeTodoNotificationService);

        var todos = todoRepository.GetAll();

        var initialTodoCount = todos.Count();

        string title = "test";

        // Act

        var createTodoCommand = new CreateTodo(title, null, Dtos.TodoStatusDto.NotStarted);

        var result = await commandHandler.Handle(createTodoCommand, default);

        // Assert

        Assert.True(result.IsSuccess);

        var todo = result.GetValue();

        todos = todoRepository.GetAll();

        var newTodoCount = todos.Count();

        newTodoCount.Should().BeGreaterThan(initialTodoCount);

        // Has Domain Event been published ?

        await fakeDomainEventDispatcher
            .Received(1)
            .Dispatch(Arg.Is<TodoCreated>(d => d.TodoId == todo.Id));
    }
}
