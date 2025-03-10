namespace GoalTracker.UI.Blazor.Models;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Username { get; set; }
}

public class RegisterResponse
{
    public bool Success { get; set; }
    public string[] Errors { get; set; }
}