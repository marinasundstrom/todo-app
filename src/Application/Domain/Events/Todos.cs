using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Events;

public sealed record TodoAssignedUserUpdated(TodoId TodoId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;

public sealed record TodoCreated(TodoId TodoId) : DomainEvent;

public sealed record TodoDeleted(TodoId TodoId, string Title) : DomainEvent;

public sealed record TodoDescriptionUpdated(TodoId TodoId, string? Description) : DomainEvent;

public sealed record TodoEstimatedHoursUpdated(TodoId TodoId, double? Hours, double? OldHours) : DomainEvent;

public sealed record TodoRemainingHoursUpdated(TodoId TodoId, double? hHurs, double? OldHours) : DomainEvent;

public sealed record TodoStatusUpdated(TodoId TodoId, TodoStatus NewStatus, TodoStatus OldStatus) : DomainEvent;

public sealed record TodoTitleUpdated(TodoId TodoId, string Title) : DomainEvent;

public sealed record TodoUpdated(TodoId TodoId) : DomainEvent;