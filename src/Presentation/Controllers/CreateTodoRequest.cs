using TodoApp.Application.Todos.Dtos;

namespace TodoApp.Presentation.Controllers;

public record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status);
