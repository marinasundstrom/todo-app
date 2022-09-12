using Microsoft.AspNetCore.Builder;
using TodoApp.Presentation.Hubs;

namespace TodoApp.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
