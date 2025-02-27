using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoalTracker.UI.Blazor;
using GoalTracker.UI.Blazor.Services.Base;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Services;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//Microsoft.extensions.htt
builder.Services.AddHttpClient<IClient,Client>(Client => Client.BaseAddress = new Uri(
    "https://localhost:7124"));

builder.Services.AddScoped<IGoalService,GoalService>();
builder.Services.AddScoped<IWorkItemService,WorkItemService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

await builder.Build().RunAsync();
