using Microsoft.AspNetCore.Builder;
using TodoApp.Application.Features.Todos;
using TodoApp.Application.Features.Users;

namespace TodoApp.Application.Extensions;

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
