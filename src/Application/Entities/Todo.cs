﻿using TodoApp.Application.Enums;
using TodoApp.Application.Events;

namespace TodoApp.Application.Entities;

public class Todo : AggregateRoot<int>, IAuditable
{
    protected Todo() : base(0)
    {
    }

    public Todo(string title, string? description, TodoStatus status = TodoStatus.NotStarted)
        : base(0)
    {
        Title = title;
        Description = description;
        Status = status;
    }

    public string Title { get; private set; } = null!;

    public bool UpdateTitle(string title)
    {
        var oldTitle = Title;
        if (title != oldTitle)
        {
            Title = title;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoTitleUpdated(Id, title));

            return true;
        }

        return false;
    }

    public string? Description { get; private set; }

    public bool UpdateDescription(string? description)
    {
        var oldDescription = Description;
        if (description != oldDescription)
        {
            Description = description;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoDescriptionUpdated(Id, description));

            return true;
        }

        return false;
    }

    public TodoStatus Status { get; private set; }

    public bool UpdateStatus(TodoStatus status)
    {
        var oldStatus = Status;
        if (status != oldStatus)
        {
            Status = status;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoStatusUpdated(Id, status, oldStatus));

            return true;
        }

        return false;
    }

    public User? AssignedTo { get; private set; }

    public string? AssignedToId { get; private set; }

    public bool UpdateAssignedTo(string? userId)
    {
        var oldAssignedToId = AssignedToId;
        if (userId != oldAssignedToId)
        {
            AssignedToId = userId;
            AddDomainEvent(new TodoAssignedUserUpdated(Id, userId, oldAssignedToId));

            return true;
        }

        return false;
    }

    public double? EstimatedHours { get; private set; }

    public bool UpdateEstimatedHours(double? hours)
    {
        var oldHours = EstimatedHours;
        if (hours != oldHours)
        {
            EstimatedHours = hours;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoEstimatedHoursUpdated(Id, hours, oldHours));

            return true;
        }

        return false;
    }

    public double? RemainingHours { get; private set; }

    public bool UpdateRemainingHours(double? hours)
    {
        var oldHours = RemainingHours;
        if (hours != oldHours)
        {
            RemainingHours = hours;

            AddDomainEvent(new TodoUpdated(Id));
            AddDomainEvent(new TodoRemainingHoursUpdated(Id, hours, oldHours));

            return true;
        }

        return false;
    }

    public User CreatedBy { get; set; } = null!;
    public string CreatedById { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }
    public string? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}