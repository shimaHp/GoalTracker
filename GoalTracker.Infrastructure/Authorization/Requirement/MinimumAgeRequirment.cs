

using Microsoft.AspNetCore.Authorization;

namespace GoalTracker.Infrastructure.Authorization.Requirement;

public class MinimumAgeRequirment(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
