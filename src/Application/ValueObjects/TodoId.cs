using System.Diagnostics.CodeAnalysis;

namespace TodoApp.Application.ValueObjects;

public struct TodoId
{
    public TodoId(int value) => Value = value;

    public int Value { get; set; }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(TodoId lhs, TodoId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(TodoId lhs, TodoId rhs) => lhs.Value != rhs.Value;

    public static implicit operator TodoId(int id) => new TodoId(id);

    public static implicit operator TodoId?(int? id) => id is null ? (TodoId?)null : new TodoId(id.GetValueOrDefault());

    public static implicit operator int(TodoId id) => id.Value;
}
