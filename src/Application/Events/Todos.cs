namespace TodoApp.Application.Events;

public sealed record TodoAssignedUserUpdated(int TodoId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;

public sealed record TodoCreated(int TodoId) : DomainEvent;

public sealed record TodoDeleted(int TodoId, string Title) : DomainEvent;

public sealed record TodoDescriptionUpdated(int TodoId, string? Description) : DomainEvent;

public sealed record TodoEstimatedHoursUpdated(int TodoId, double? Hours, double? OldHours) : DomainEvent;

public sealed record TodoRemainingHoursUpdated(int TodoId, double? hHurs, double? OldHours) : DomainEvent;

public sealed record TodoStatusUpdated(int TodoId, TodoStatus NewStatus, TodoStatus OldStatus) : DomainEvent;

public sealed record TodoTitleUpdated(int TodoId, string Title) : DomainEvent;

public sealed record TodoUpdated(int TodoId) : DomainEvent;