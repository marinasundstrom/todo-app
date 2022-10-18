using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using TodoApp;
using TodoApp.Theming;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri("https://localhost:5001/"));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddHttpClient<ITodosClient>(nameof(TodosClient), (sp, http) =>
{
    http.BaseAddress = new Uri("https://localhost:5001/");
})
.AddTypedClient<ITodosClient>((http, sp) => new TodosClient(http))
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddHttpClient<IUsersClient>(nameof(UsersClient), (sp, http) =>
{
    http.BaseAddress = new Uri("https://localhost:5001/");
})
.AddTypedClient<IUsersClient>((http, sp) => new UsersClient(http))
.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
//.SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
//.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<TodoApp.Services.IAccessTokenProvider, TodoApp.Services.AccessTokenProvider>();

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddThemeServices();

builder.Services.AddLocalization();

var app = builder.Build();

await app.Services.Localize();

await app.RunAsync();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
         .HandleTransientHttpError()
         .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5));
}