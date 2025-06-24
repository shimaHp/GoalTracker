
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.Goals;
using GoalTracker.UI.Blazor.Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Interfaces.Services
{
    public interface IGoalService
    {
        Task<List<GoalViewModel>> GetGoals();
        Task<PagedResult<GoalViewModel>> GetGoals(string searchPhrase = "", int pageNumber = 1, int pageSize = 10, string sortBy = "Title", int sortDirection = 0);
        //Task<GoalViewModel> GetGoalDetail(int id);
        Task<DetailGoalViewModel> GetGoalDetail(int id);
       
        Task<Response<GoalDto>> CreateGoal(CreateGoalViewModel goal);
        //Task<Response<Guid>> CreateGoal(CreateGoalViewModel goal);

    public GoalViewModel CachedGoal { get; set; }
    public DetailGoalViewModel CachedDetailGoal { get; set; }
        Task<Response<GoalViewModel>> UpdateGoal(int id, UpdateGoalViewModel goalViewModel);

       // Task<Response<GoalDto>> UpdateGoal(int id, UpdateGoalViewModel goalViewModel);
        Task<Response<int>> DeleteGoal(int goalId);
        
    }
}
