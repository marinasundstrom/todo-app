namespace TodoApp.Application.Todos.Dtos;

public sealed record TodoDto(int Id, string Title, string? Description, double? EstimatedHours, double? RemainingHours, TodoStatusDto Status, DateTimeOffset Created, DateTimeOffset? LastModified);
