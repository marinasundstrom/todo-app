using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.SignalR.Client;

namespace TodoApp.IntegrationTests;

partial class TodosTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task ShouldGetNotificationWhenTodoIsCreated()
    {
        // Arrange

        var client = _factory.CreateClient();

        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"http://localhost/hubs/todos", o => o.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler())
            .WithAutomaticReconnect().Build();

        var completion = new ManualResetEvent(false);

        int? receivedId = null;

        hubConnection.On<int>("Created", (id) =>
        {
            receivedId = id;
            completion.Set();
        });

        await hubConnection.StartAsync();

        TodosClient todosClient = new(client);

        string title = "Foo Bar";
        string description = "Lorem ipsum";
        TodoStatusDto status = TodoStatusDto.InProgress;

        // Act

        var todo = await todosClient.CreateTodoAsync(new CreateTodoRequest()
        {
            Title = title,
            Description = description,
            Status = status
        });

        completion.WaitOne();

        await hubConnection.StopAsync();

        // Assert

        receivedId.Should().NotBeNull();
        receivedId.Should().Be(todo.Id);
    }
}