
using GoalTracker.Application.Users;
using GoalTracker.Domain;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Infrastructure.Authorization.Services;

public class GoalAuthorizationService(ILogger<GoalAuthorizationService> logger, IUserContext userContext) : IGoalAuthorizationService
{
 
    public bool Authorize(Goal goal, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation(
            "Authorizing user {UserEmail} to {Operation} for goal {GoalTitle}",
            user.Email, resourceOperation, goal.Title);

        // Admin has full access
        if (user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user - successful authorization");
            return true;
        }

        // Read operations allowed for all roles
        if (resourceOperation == ResourceOperation.Read)
        {
            logger.LogInformation("Read Operation - successful authorization");
            return true;
        }

        // Owner role can create, update, and delete their own goals
        if (user.IsInRole(UserRoles.Owner) &&
            (resourceOperation == ResourceOperation.Create ||
             (goal.UserId == user.Id &&
              (resourceOperation == ResourceOperation.Update ||
               resourceOperation == ResourceOperation.Delete))))
        {
            logger.LogInformation("Goal Owner - successful authorization");
            return true;
        }

        // Viewer can only read
        if (user.IsInRole(UserRoles.Viewer) && resourceOperation != ResourceOperation.Read)
        {
            logger.LogWarning(
                "Authorization failed: Viewer attempted {Operation}",
                resourceOperation);
            return false;
        }
        // Allow Collaborators to create or update goals
        if (user.IsInRole(UserRoles.Collaborator) &&
            (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.Update))
        {
            logger.LogInformation("Collaborator - authorized to create or update goals");
            return true;
        }

        logger.LogWarning(
            "Authorization failed for user {UserEmail} on goal {GoalTitle} with operation {Operation}",
            user.Email, goal.Title, resourceOperation);
        return false;
    }
}
