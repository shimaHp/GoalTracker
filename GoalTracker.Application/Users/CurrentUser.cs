

namespace GoalTracker.Application.Users;

public record CurrentUser(string Id,string UserName, string Email, IEnumerable<string> Roles, DateOnly? DateOfBirth)
{
    public bool IsInRole(string role) =>
     Roles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
}
