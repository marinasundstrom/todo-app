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
        Assert.Equal(description, todo.Description);
        Assert.Equal(status, todo.Status);

        Assert.Equal(todo.Id, todo2.Id);
        Assert.Equal(todo.Title, todo2.Title);
        Assert.Equal(todo.Description, todo2.Description);
        Assert.Equal(todo.Status, todo2.Status);
        Assert.Equal(todo.Created, todo2.Created);
        Assert.Equal(todo.LastModified, todo2.LastModified);
    }
}