﻿namespace TodoApp.Domain.Entities;

public interface ISoftDelete
{
    string? DeletedById { get; set; }
    DateTimeOffset? Deleted { get; set; }
}
