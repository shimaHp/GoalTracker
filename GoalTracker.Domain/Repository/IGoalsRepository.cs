

using GoalTracker.Domain.Entities;

namespace GoalTracker.Domain.Repository;

public interface IGoalsRepository
{
    Task<IEnumerable<Goal>> GetAllAsync();

    Task<Goal?> GetGoalAsync(int GId);
}
