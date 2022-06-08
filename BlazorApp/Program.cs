using BlazorApp;
using BlazorApp.WellKnownNames;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration[AppSettings.ApiBaseUrl] ?? "";
builder.Services.AddHttpClient(AppSettings.HttpClientApi, (services, client) =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(AppSettings.HttpClientApi));
builder.Services.AddLocalization();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
