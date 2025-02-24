
using GoalTracker.Domain.Constants;
using GoalTracker.Domain;
using GoalTracker.Domain.Entities;
using GoalTracker.Application.Users;
using Microsoft.Extensions.Logging;
using GoalTracker.Domain.Interfaces;

namespace GoalTracker.Infrastructure.Authorization.Services
{
    public class WorkItemAuthorizationService(Microsoft.Extensions.Logging.ILogger<WorkItemAuthorizationService> logger, IUserContext userContext): IWorkItemAuthorizationService
    {
        public bool Authorize(WorkItem workItem, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation(
            "Authorizing user {UserEmail} to {Operation} for work item {WorkItemTitle}",
            user.Email, resourceOperation, workItem.Title);

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

        // Owner role specific logic
        if (user.IsInRole(UserRoles.Owner))
        {
            // Owner can create work items
            if (resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Work Item Creation by Owner - successful authorization");
                return true;
            }

            // Owner can update/delete work items they created or are assigned to
            if ((resourceOperation == ResourceOperation.Update ||
                 resourceOperation == ResourceOperation.Delete) &&
                (workItem.CreatorId == user.Id ||
                 workItem.AssigneeId == user.Id))
            {
                logger.LogInformation("Work Item Owner - successful authorization");
                return true;
            }
        }

        // Viewer can only read
        if (user.IsInRole(UserRoles.Viewer) && resourceOperation != ResourceOperation.Read)
        {
            logger.LogWarning(
                "Authorization failed: Viewer attempted {Operation}",
                resourceOperation);
            return false;
        }

        logger.LogWarning(
            "Authorization failed for user {UserEmail} on work item {WorkItemTitle} with operation {Operation}",
            user.Email, workItem.Title, resourceOperation);
        return false;
    }

        
    }
}
