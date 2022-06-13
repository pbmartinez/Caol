using BlazorApp;
using BlazorApp.WellKnownNames;
using Domain.Interfaces;
using Domain.Services;
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
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddScoped<IGlobalizationService, GlobalizationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

await builder.Build().RunAsync();
