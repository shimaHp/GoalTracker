
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Models.ViewModels;
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
        Task<GoalViewModel> GetGoalDetail(int id);
       
        Task<Response<GoalDto>> CreateGoal(CreateGoalViewModel goal);
        //Task<Response<Guid>> CreateGoal(CreateGoalViewModel goal);

        Task<Response<Guid>> UpdateGoal(int id,GoalViewModel goal);
        Task<Response<Guid>> DeleteGoal(GoalViewModel goal);
        
    }
}
