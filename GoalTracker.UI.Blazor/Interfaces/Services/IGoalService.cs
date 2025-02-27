using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.Interfaces.Services
{
    public interface IGoalService
    {
        Task<List<GoalViewModel>> GetGoals();
        Task<GoalViewModel> GetGoalDetail(int id);
        Task<Response<Guid>> CreateGoal(GoalViewModel goal);
        Task<Response<Guid>> UpdateGoal(int id,GoalViewModel goal);
        Task<Response<Guid>> DeleteGoal(GoalViewModel goal);
        
    }
}
