using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.Goals;
using Microsoft.AspNetCore.Components;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class GoalDetails
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IGoalService GoalService { get; set; }

        [Parameter]
        public int Id { get; set; }

        //public GoalViewModel Goal { get; private set; }
        public DetailGoalViewModel Goal { get; private set; }
        public string Message { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = true;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Goal = await GoalService.GetGoalDetail(Id);
                if (Goal == null)
                {
                    Message = "Goal not found";
                }
            }
            catch (Exception ex)
            {
                Message = $"Error loading goal: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void EditGoal()
        {
            NavigationManager.NavigateTo($"/Goals/update/{Id}");
        }

        protected void GoBack()
        {
            NavigationManager.NavigateTo("/Goals");
        }
    }
}