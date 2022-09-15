using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TodoApp;
using TodoApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<ITodosClient>((sp, https) =>
{
    https.BaseAddress = new Uri("https://localhost:5001/");
})
.AddTypedClient<ITodosClient>((http, sp) => new TodosClient(http));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
