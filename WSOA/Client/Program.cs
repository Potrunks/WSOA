using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WSOA.Client;
using WSOA.Client.Services.Implementation;
using WSOA.Client.Services.Interface;
using WSOA.Shared.Stores;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();

builder.Services.AddSingleton<TournamentInProgressStore>();

builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
