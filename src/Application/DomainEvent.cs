﻿using MediatR;

namespace TodoApp.Application;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}