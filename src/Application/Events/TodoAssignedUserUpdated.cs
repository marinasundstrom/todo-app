namespace TodoApp.Application.Events;

public sealed record TodoAssignedUserUpdated(int TodoId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;