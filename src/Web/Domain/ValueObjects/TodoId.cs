namespace TodoApp.Domain.ValueObjects;

public readonly struct TodoId
{
    public TodoId(int value) => Value = value;

    public int Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator TodoId(int id) => new TodoId(id);

    public static implicit operator TodoId?(int? id) => id is null ? (TodoId?)null : new TodoId(id.GetValueOrDefault());

    public static implicit operator int(TodoId id) => id.Value;
}
