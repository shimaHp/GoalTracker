

using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Pages.Authentication;




public partial class Login
{
    

    [Inject]
    private NavigationManager NavigationManager { get; set; }
    public LoginViewModel Model { get; set; }
    private string Message { get; set; }

    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }

    protected override void OnInitialized()
    {
        Model= new LoginViewModel();
    }
    protected async Task HandleLogin()
    {
        // Clear any previous error message
        Message = string.Empty;

        try
        {
            // Attempt authentication
            var result = await AuthenticationService.AuthenticateAsync(Model.Email, Model.Password);

            if (result)
            {
                
                // Success - navigate to home page
                NavigationManager.NavigateTo("/");
                return; // Important to return here
            }

            // Authentication failed
            Message = "Username/password combination unknown";
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Message = $"An error occurred: {ex.Message}";
        }
    }
}
