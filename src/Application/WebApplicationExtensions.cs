using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using TodoApp.Application.Features.Todos;
using TodoApp.Application.Features.Users;

namespace TodoApp.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.AddTodoEndpoints();
        app.AddUsersEndpoints();

        return app;
    }

    public static WebApplication MapApplicationHubs(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
