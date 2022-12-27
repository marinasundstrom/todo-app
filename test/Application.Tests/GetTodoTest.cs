using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoApp.Application.Services;
using TodoApp.Application.Todos.Queries;
using TodoApp.Domain;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Repositories;
using TodoApp.Infrastructure.Persistence.Repositories.Mocks;
using Xunit;

namespace TodoApp.Application.Todos.Commands;

public class GetTodoTest
{
    [Fact]
    public async Task GetTodo_TodoNotFound()
    {
        // Arrange

        var fakeDomainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        using (var connection = new SqliteConnection("Data Source=:memory:"))
        {
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var unitOfWork = new ApplicationDbContext(dbContextOptions);

            await unitOfWork.Database.EnsureCreatedAsync();

            var todoRepository = new TodoRepository(unitOfWork);

            var commandHandler = new GetTodoById.Handler(todoRepository);

            int nonExistentTodoId = 9999;

            // Act

            var getTodoByIdCommand = new GetTodoById(nonExistentTodoId);

            var result = await commandHandler.Handle(getTodoByIdCommand, default);

            // Assert

            Assert.True(result.HasError(Errors.Todos.TodoNotFound));
        }
    }
}
