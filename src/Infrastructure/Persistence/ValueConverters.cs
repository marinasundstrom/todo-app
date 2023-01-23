using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Application.ValueObjects;

namespace TodoApp.Infrastructure.Persistence.ValueConverters;

internal sealed class TodoIdConverter : ValueConverter<TodoId, int>
{
    public TodoIdConverter()
        : base(v => v.Value, v => new(v))
        {
        }
}

internal sealed class UserIdConverter : ValueConverter<UserId, string>
{
    public UserIdConverter()
        : base(v => v.Value, v => new(v))
        {
        }
}