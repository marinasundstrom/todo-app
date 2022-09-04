using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TodoApp.Infrastructure.Persistance;

namespace TodoApp.IntegrationTests;

public class TodosTest
{
    [Fact]
    public async Task CreatedTodoShouldBeRetrieved()
    {
        // Arrange

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseSqlite("Data Source=testdb.db");
                    });

                    services.AddMassTransitTestHarness(cfg =>
                    {
                        //cfg.AddConsumer<SubmitOrderConsumer>();
                    });
                });
            });

        var client = application.CreateClient();
        //...

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
