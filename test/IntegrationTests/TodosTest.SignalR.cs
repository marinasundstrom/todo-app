using System.Net.Http.Headers;
using Microsoft.AspNetCore.SignalR.Client;

namespace TodoApp.IntegrationTests;

partial class TodosTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task ShouldGetNotificationWhenTodoIsCreated()
    {
        // Arrange

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("JWT");

        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"http://localhost/hubs/todos", o => o.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler())
            .WithAutomaticReconnect().Build();

        var completion = new ManualResetEvent(false);

        int? receivedId = null;

        hubConnection.On<int, string>("Created", (id, title) =>
        {
            receivedId = id;
            completion.Set();
        });

        await hubConnection.StartAsync();

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

        completion.WaitOne();

        await hubConnection.StopAsync();

        // Assert

        receivedId.Should().NotBeNull();
        receivedId.Should().Be(todo.Id);
    }
}