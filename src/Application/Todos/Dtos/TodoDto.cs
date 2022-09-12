namespace TodoApp.Application.Todos.Dtos;

public record TodoDto(int Id, string Title, string? Description, double? EstimatedHours, double? RemainingHours, TodoStatusDto Status, DateTime Created, DateTime? LastModified);
