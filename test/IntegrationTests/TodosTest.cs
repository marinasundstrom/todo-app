using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TodoApp.IntegrationTests;

public class TodosTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TodosTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreatedTodoShouldBeRetrieved()
    {
        // Arrange
        var client = _factory.CreateClient();

        TodosClient todosClient = new(client);

        string title = "Foo Bar";
        string description = "Lorem ipsum";
        TodoStatusDto status = TodoStatusDto.Ongoing;


        // Act

        var todo = await todosClient.CreateTodoAsync(new CreateTodoRequest()
        {
            Title = title,
            Description = description,
            Status = status
        });

        var todo2 = await todosClient.GetTodoByIdAsync(todo.Id);

        // Assert

        Assert.Equal(title, todo.Title);

        todo.Title.Should().Be(title);
        todo.Description.Should().Be(description);
        todo.Status.Should().Be(status);

        todo2.Id.Should().Be(todo.Id);
        todo2.Title.Should().Be(todo.Title);
        todo2.Description.Should().Be(todo.Description);
        todo2.Status.Should().Be(todo.Status);
        todo2.Created.Should().Be(todo.Created);
        todo2.LastModified.Should().Be(todo.LastModified);
    }
}