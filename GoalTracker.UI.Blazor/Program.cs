using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoalTracker.UI.Blazor;
using GoalTracker.UI.Blazor.Services.Base;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Services;
using System.Reflection;
using Serilog;
using GoalTracker.UI.Blazor.MappingProfiles;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using GoalTracker.UI.Blazor.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//Microsoft.extensions.htt
builder.Services.AddHttpClient<IClient,Client>(Client => Client.BaseAddress = new Uri(
    "https://localhost:7124"));
//todo
// Configure Serilog to log to the browser's console


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IGoalService, GoalService>();
//builder.Services.AddScoped<IWorkItemService,WorkItemService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();
