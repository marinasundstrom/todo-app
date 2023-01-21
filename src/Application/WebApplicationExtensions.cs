using Microsoft.AspNetCore.Builder;
using TodoApp.Application.Features.Todos;

namespace TodoApp.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
