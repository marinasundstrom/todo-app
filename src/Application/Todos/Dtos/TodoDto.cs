namespace TodoApp.Application.Todos.Dtos;

using TodoApp.Application.Users;

public sealed record TodoDto(int Id, string Title, string? Description, double? EstimatedHours, double? RemainingHours, TodoStatusDto Status, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
