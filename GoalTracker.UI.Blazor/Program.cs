using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoalTracker.UI.Blazor;
using GoalTracker.UI.Blazor.Services.Base;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Services;
using System.Reflection;
using Serilog;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using GoalTracker.UI.Blazor.Providers;
using GoalTracker.UI.Blazor.MappingProfiles.Goals;
using System;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
builder.Services.AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri("https://localhost:7124"));

// Add Blazored services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast(); // Add Blazored Toast here

// Add Authentication
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(GoalMappingProfile));

// Add your services
builder.Services.AddScoped<IGoalService, GoalService>();
//builder.Services.AddScoped<IWorkItemService, WorkItemService>();

// Build and run the app
var app = builder.Build();
await app.RunAsync();