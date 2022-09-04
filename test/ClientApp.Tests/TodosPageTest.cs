using Bunit;
using ClientApp.Pages;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using TodoApp;

namespace ClientApp.Tests;

public class TodosPageTest
{
    [Fact]
    public void ItemsShouldLoadOnInitializedSuccessful()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Services.AddMudServices();

        var fakeTodosClient = Substitute.For<ITodosClient>();
        fakeTodosClient.GetTodosAsync(Arg.Any<TodoStatusDto>(), null, null, null, null)
            .ReturnsForAnyArgs(t => new ItemsResultOfTodoDto()
            {
                Items = new[]
                {
                    new TodoDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Item 1",
                        Description = "Description",
                        Status = TodoStatusDto.Ongoing,
                        Created = DateTimeOffset.Now.AddMinutes(-3)
                    },
                    new TodoDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Item 2",
                        Description = "Description",
                        Status = TodoStatusDto.Ongoing,
                        Created = DateTimeOffset.Now.AddMinutes(-1)
                    },
                    new TodoDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Item 2",
                        Description = "Description",
                        Status = TodoStatusDto.Ongoing,
                        Created = DateTimeOffset.Now
                    }
                },
                TotalItems = 3
            });

        ctx.Services.AddSingleton<ITodosClient>(fakeTodosClient);

        var cut = ctx.RenderComponent<TodosPage>();

        // Act
        //cut.Find("button").Click();

        // Assert
        cut.WaitForState(() => cut.Find("tr") != null);

        int expectedNoOfTr = 4; // incl <td> in <thead>

        cut.FindAll("tr").Count.Should().Be(expectedNoOfTr);
    }
}
