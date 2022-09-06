namespace TodoApp.Domain;

public class Result
{
    private Error? error;

    private Result()
    {
    }

    protected Result(Error? error)
    {
        this.error = error;
    }

    public static Result Success() => new Result();

    public static Result Error(Error error) => new Result(error);

    public static Result<T> Success<T>(T data) => new(data);

    public static Result<T> Error<T>(Error error) => new(error);

    public bool IsSuccess() => error is null;

    public bool HasError() => error is not null;

    public bool HasError(Error error) => error is not null && this.error == error;

    public bool HasError<T>(T error) where T : Error => this.error is T;

    public Error? GetError() => error;

    public T? GetError<T>() where T : Error => (T?)error;

    public static implicit operator Error(Result result) =>
        !result.HasError()
        ? throw new InvalidOperationException() : result.error!;
}

public class Result<T> : Result
{
    private T? data { get; }

    public Result(T Data) : base(null)
    {
        data = Data;
    }

    public Result(Error error) : base(error: error)
    {
    }

    public T GetValue() => data!;

    public static implicit operator T(Result<T> result) =>
        result.HasError()
        ? throw new InvalidOperationException() : result.data!;
}
