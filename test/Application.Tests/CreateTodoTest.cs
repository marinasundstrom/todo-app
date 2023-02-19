using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoApp.Application.Events;
using TodoApp.Application.Features.Todos;
using TodoApp.Application.Features.Todos.Commands;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Interceptors;
using TodoApp.Infrastructure.Persistence.Repositories;

namespace TodoApp.Application.Todos.Commands;

public class CreateTodoTest
{
    [Fact]
    public async Task CreateTodo_TodoCreated()
    {
        // Arrange

        var fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeCurrentUserService.UserId.Returns("foo");

        var fakeDateTimeService = Substitute.For<IDateTime>();
        fakeDateTimeService.Now.Returns(DateTime.UtcNow);

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        var fakeTodoNotificationService = Substitute.For<ITodoNotificationService>();

        using (var connection = new SqliteConnection("Data Source=:memory:"))
        {
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .AddInterceptors(new AuditableEntitySaveChangesInterceptor(fakeCurrentUserService, fakeDateTimeService), new FakeOutboxSaveChangesInterceptor(fakeDomainEventDispatcher))
                .UseSqlite(connection)
                .Options;

            using var unitOfWork = new ApplicationDbContext(dbContextOptions);

            await unitOfWork.Database.EnsureCreatedAsync();

            unitOfWork.Users.Add(new Entities.User("foo", "Test Tesston", "test@foo.com"));

            await unitOfWork.SaveChangesAsync();

            var todoRepository = new TodoRepository(unitOfWork);

            var commandHandler = new CreateTodo.Handler(todoRepository, unitOfWork, fakeDomainEventDispatcher);

            var todos = todoRepository.GetAll();

            var initialTodoCount = todos.Count();

            string title = "test";

            // Act

            var createTodoCommand = new CreateTodo(title, null, TodoStatusDto.NotStarted, null, 0, 0);

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
}
