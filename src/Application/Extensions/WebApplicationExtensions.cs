using Microsoft.AspNetCore.Builder;
using TodoApp.Features.Todos;
using TodoApp.Features.Users;

namespace TodoApp.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapApplicationEndpoints(this WebApplication app)
    {
        app.MapTodoEndpoints();
        app.MapUsersEndpoints();

        return app;
    }

    public static WebApplication MapApplicationHubs(this WebApplication app)
    {
        app.MapTodoHubs();

        return app;
    }
}
