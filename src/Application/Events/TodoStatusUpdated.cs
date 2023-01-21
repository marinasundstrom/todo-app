using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Events;

public sealed record TodoStatusUpdated(int TodoId, TodoStatus NewStatus, TodoStatus OldStatus) : DomainEvent;