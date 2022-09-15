namespace TodoApp.Services;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
