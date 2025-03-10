using GoalTracker.UI.Blazor.Dtos;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Services;
using Microsoft.AspNetCore.Components;
namespace GoalTracker.UI.Blazor.Pages.Authentication
{
    public partial class Register
    {
        [Inject]
        private IAuthenticationService AuthService { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }


        private RegisterRequestDto registerRequest = new();
        private string message;
        private bool isSuccess;

        //    protected async Task HandleRegistration()
        //    {
        //        message = null;

        //        var result = await AuthService.Register(registerRequest);

        //        if (result)
        //        {
        //            isSuccess = true;
        //            message = "Registration successful! You can now login.";
        //            registerRequest.UserName = "";
        //        }
        //        else
        //        {
        //            isSuccess = false;
        //            message = "Registration failed. Please try again.";
        //        }
        //    }
        //}
    }
}