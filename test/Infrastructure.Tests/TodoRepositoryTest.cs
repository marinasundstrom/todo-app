using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Persistence.Repositories;

namespace TodoApp.Infrastructure;

public class TodoRepositoryTest
    : IClassFixture<TodoFixture>
{
    private readonly TodoFixture fixture;

    public TodoRepositoryTest(TodoFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task TodoShouldBeAdded()
    {
        var context = fixture.CreateDbContext();
        var todoRepository = new TodoRepository(context);

        var todo = new Todo("Test", "Desc");
        todoRepository.Add(todo);

        await todoRepository.SaveChangesAsync();

        var todo2 = await todoRepository.FindByIdAsync(todo.Id);

        todo2.Should().NotBeNull();
        todo2!.Id.Should().Be(todo.Id);
    }

    [Fact]
    public async Task AllTodosShouldBeRetrieved()
    {
        var context = fixture.CreateDbContext();
        var todoRepository = new TodoRepository(context);

        var todo = new Todo("Test1", "Desc");
        todoRepository.Add(todo);

        var todo2 = new Todo("Test2", "Desc");
        todoRepository.Add(todo2);

        await todoRepository.SaveChangesAsync();

        var todos = await todoRepository.GetAll().ToListAsync();

        todos.Count.Should().Be(2);
    }
}
