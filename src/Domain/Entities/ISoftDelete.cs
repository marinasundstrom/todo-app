namespace TodoApp.Domain.Entities;

public interface ISoftDelete
{
    string? DeletedById { get; set; }
    DateTime? Deleted { get; set; }
}
