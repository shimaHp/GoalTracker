using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Application.Goals
{
    public interface IGoalsService
    {
        Task<int> CreateGoal(CreateGoalDto dto);
        Task<IEnumerable<GoalDto>> GetAllGoals();
        Task<GoalDto?> GetGoalById(int GId);
    }
}