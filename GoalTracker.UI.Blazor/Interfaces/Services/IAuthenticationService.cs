

using GoalTracker.UI.Blazor.Dtos;
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Services.Base;

using LoginRequest = GoalTracker.UI.Blazor.Services.Base.LoginCommand;


namespace GoalTracker.UI.Blazor.Interfaces.Services;

public interface IAuthenticationService
{
    Task<string> Login(LoginRequestDto loginRequest);
    Task Logout();
    Task<string> Register(RegisterRequestDto registerRequest);
 
    Task<bool> AuthenticateAsync(string email, string password);
}