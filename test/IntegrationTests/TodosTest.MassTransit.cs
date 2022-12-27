using System;
using System.Net.Http.Headers;
using MassTransit;
using MassTransit.Testing;
using TodoApp.Consumers;
using TodoApp.Contracts;
using static MassTransit.Logging.OperationName;

namespace TodoApp.IntegrationTests;

partial class TodosTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    [Fact]
    public async Task UpdateStatusConsumed()
    {
        // Arrange

        var harness = _factory.Services.GetTestHarness();

        await harness.Start();

        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("JWT");

        TodosClient todosClient = new(client);

        string title = "Foo Bar";
        string description = "Lorem ipsum";
        Contracts.TodoStatus status = Contracts.TodoStatus.InProgress;

        var newStatus = Contracts.TodoStatus.Completed;

        var todo = await todosClient.CreateTodoAsync(new CreateTodoRequest()
        {
            Title = title,
            Description = description,
            Status = status
        });

        // Act

        await harness.Bus.Publish(
            new UpdateStatus(todo.Id, (Contracts.TodoStatus)newStatus));

        // Assert

        Assert.True(await harness.Consumed.Any<UpdateStatus>());

        var todo2 = await todosClient.GetTodoByIdAsync(todo.Id);

        todo2.Status.Should().Be(newStatus);
    }
}

