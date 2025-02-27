using AutoMapper;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services.Base;

namespace GoalTracker.UI.Blazor.Services
{
    public class GoalService(IClient client, IMapper mapper) : BaseHttpService(client), IGoalService
    {
        public async Task<List<GoalViewModel>> GetGoals()
        {
            var goals = await client.GoalsAllAsync("", 1, 10, "", Base.SortDirection._0);
            return mapper.Map<List<GoalViewModel>>(goals);
        }

        public async Task<GoalViewModel> GetGoalDetail(int id)
        {
            var goal = await client.GoalsAll2Async(id);
            return mapper.Map<GoalViewModel>(goal);
        }

        public async Task<Response<Guid>> CreateGoal(GoalViewModel goal)
        {
            try
            {
                var createGoalCommand = mapper.Map<CreateGoalCommand>(goal);
                await client.GoalsPOSTAsync(createGoalCommand);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex) { 
            return ConvertApiExceptions<Guid>(ex);
            }

           
        }

        public async Task<Response<Guid>> DeleteGoal(GoalViewModel goal)
        {
            try
            {
                
                await client.GoalsDELETEAsync(goal.Id);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<Response<Guid>> UpdateGoal(int id, GoalViewModel goal)
        {
            try
            {
                var updateGoalTypeCommand = mapper.Map<UpdateGoalCommand>(goal);
                await client.GoalsPATCHAsync(id, updateGoalTypeCommand);
                return new Response<Guid> { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }
}
