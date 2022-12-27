using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using TodoApp;
using TodoApp.Pages;

namespace TodoApp.Tests;

public class TodosPageTest
{
    [Fact]
    public void ItemsShouldLoadOnInitializedSuccessful()
    {
        // Arrange
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Services.AddMudServices();

        ctx.Services.AddLocalization();

        var fakeAccessTokenProvider = Substitute.For<TodoApp.Services.IAccessTokenProvider>();

        ctx.Services.AddSingleton(fakeAccessTokenProvider);

        var fakeTodosClient = Substitute.For<ITodosClient>();
        fakeTodosClient.GetTodosAsync(Arg.Any<TodoStatus>(), null, null, null, null, default)
            .ReturnsForAnyArgs(t => new ItemsResultOfTodo()
            {
                Items = new[]
                {
                    new Todo
                    {
                        Id = 1,
                        Title = "Item 1",
                        Description = "Description",
                        Status = TodoStatus.InProgress,
                        Created = DateTimeOffset.Now.AddMinutes(-3)
                    },
                    new Todo
                    {
                        Id = 2,
                        Title = "Item 2",
                        Description = "Description",
                        Status = TodoStatus.InProgress,
                        Created = DateTimeOffset.Now.AddMinutes(-1)
                    },
                    new Todo
                    {
                        Id = 3,
                        Title = "Item 2",
                        Description = "Description",
                        Status = TodoStatus.InProgress,
                        Created = DateTimeOffset.Now
                    }
                },
                TotalItems = 3
            });

        ctx.Services.AddSingleton<ITodosClient>(fakeTodosClient);

        var fakeUsersClient = Substitute.For<IUsersClient>();
        fakeUsersClient.GetUsersAsync(Arg.Any<int>(), null, null, null, null, default)
            .ReturnsForAnyArgs(t => new ItemsResultOfUser()
            {
                Items = new[]
                {
                    new User
                    {
                        Id = "foo",
                        Name = "Test"
                    }
                },
                TotalItems = 3
            });

        ctx.Services.AddSingleton<IUsersClient>(fakeUsersClient);

        var cut = ctx.RenderComponent<TodosPage>();

        // Act
        //cut.Find("button").Click();

        // Assert
        cut.WaitForState(() => cut.Find("tr") != null);

        int expectedNoOfTr = 2; // incl <td> in <thead>

        cut.FindAll("tr").Count.Should().Be(expectedNoOfTr);
    }
}
