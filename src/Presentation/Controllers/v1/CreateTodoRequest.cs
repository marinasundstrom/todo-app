using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Presentation.Controllers;

public sealed record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status);
