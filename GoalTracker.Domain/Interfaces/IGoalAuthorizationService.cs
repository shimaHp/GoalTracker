using GoalTracker.Domain.Entities;

namespace GoalTracker.Domain
{
    public interface IGoalAuthorizationService
    {
        bool Authorize(Goal goal, ResourceOperation resourceOperation);
    }
}