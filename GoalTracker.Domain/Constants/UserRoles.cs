
namespace GoalTracker.Domain.Constants;

public static class UserRoles
{
 

    public const string Collaborator = "Collaborator";//Can create and modify goals
    public const string Owner = "Owner";//Full project control
    public const string Admin = "Admin";//Complete system control
    public const string Viewer = "Viewer";//Read-only access
}
