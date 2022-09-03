namespace TodoApp.Application.Todos.Dtos;

public record TodoDto(string Id, string Title, string? Description, TodoStatusDto Status, DateTime Created, DateTime? LastModified);
