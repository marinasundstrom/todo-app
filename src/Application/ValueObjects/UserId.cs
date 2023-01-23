namespace TodoApp.Application.ValueObjects;

public readonly struct UserId
{
    public UserId(string value) => Value = value;
    
    public string Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator UserId (string id) => id is null ? default : new UserId(id);

    public static implicit operator UserId? (string? id) => id is null ? (UserId?)null : new UserId(id);

    public static implicit operator string (UserId id) => id.Value;
}
