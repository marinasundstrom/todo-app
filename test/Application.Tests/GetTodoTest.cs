using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TodoApp.Features.Todos.Queries;
using TodoApp.Services;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Persistence.Repositories;

namespace TodoApp.Todos.Commands;

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

            Assert.True(result.HasError(Domain.Errors.Todos.TodoNotFound));
        }
    }
}
