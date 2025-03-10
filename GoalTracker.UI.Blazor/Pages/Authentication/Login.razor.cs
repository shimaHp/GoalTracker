using GoalTracker.UI.Blazor.Dtos;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services;
using GoalTracker.UI.Blazor.Services.Base;
using Microsoft.AspNetCore.Components;

namespace GoalTracker.UI.Blazor.Pages.Authentication;




public partial class Login
{
    [Inject]
    private IAuthenticationService AuthService { get; set; }

    [Inject]
    private NavigationManager NavManager { get; set; }

    private LoginRequestDto loginRequest = new();
    private string errorMessage;
    public LoginViewModel LoginViewModel { get; set; }

    protected override void OnInitialized()
    {
        LoginViewModel = new LoginViewModel();
    }
    //protected async Task HandleLogin()
    //{
       
    //}
}
