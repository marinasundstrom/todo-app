using TodoApp.Application.Features.Todos;

namespace TodoApp.Presentation.Controllers;

public sealed record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status, string? AssignedTo, double? EstimatedHours, double? RemainingHours);
