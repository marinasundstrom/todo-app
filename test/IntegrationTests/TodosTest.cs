using System.Net;
using System.Net.Http.Headers;

namespace TodoApp.IntegrationTests;

public partial class TodosTest : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TodosTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateTodoShouldBeRetrievedByItsId()
    {
        // Arrange

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("JWT");

        TodosClient todosClient = new(client);

        string title = "Foo Bar";
        string description = "Lorem ipsum";
        TodoStatus status = TodoStatus.InProgress;

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
        //todo2.Created.Should().Be(todo.Created);
        todo2.LastModified.Should().Be(todo.LastModified);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("JWT");

        try
        {
            UsersClient usersClient = new(client);

            var user = await usersClient.CreateUserAsync(new CreateUser()
            {
                Name = "Test",
                Email = "test@email.com"
            });
        }
        catch { }
    }

    [Fact]
    public async Task NonExistentIdShouldReturnNotFound()
    {
        // Arrange

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("JWT");

        TodosClient todosClient = new(client);

        int nonExistentId = 99999;

        // Act

        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            var todo = await todosClient.GetTodoByIdAsync(nonExistentId);
        });

        // Assert

        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}