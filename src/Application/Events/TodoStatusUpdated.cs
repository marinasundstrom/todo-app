using TodoApp.Application.Enums;

namespace TodoApp.Application.Events;

public sealed record TodoStatusUpdated(int TodoId, TodoStatus NewStatus, TodoStatus OldStatus) : DomainEvent;