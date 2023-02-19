using Microsoft.AspNetCore.Builder;

namespace TodoApp.Features.Todos;

public static class WebApplicationExtensions
{
    public static WebApplication MapTodoHubs(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
