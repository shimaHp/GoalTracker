using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IGoalService GoalService { get; set; }
        public List<GoalViewModel> Goals { get; private set; }
        public string Message { get; set; } = string.Empty;
        protected void CreateGoal()
        {
            NavigationManager.NavigateTo("/Goals/create");
        }
        protected void UpdateGoal(int id)
        {
            NavigationManager.NavigateTo($"/Goals/upadete/{id}");
        }
        protected void GetGoalDetail(int id)
        {
            NavigationManager.NavigateTo($"/Goals/GoalDetails/{id}");
        }
        protected async void DeleteGoal(int id)
        {
            // bool confirmed = await JSRuntime.InvokeAsync<bool>(  "confirm"+ "Are you sure you want to delete this goal?");
            //if (!confirmed) return;
            var goalToDelete = Goals.FirstOrDefault(g => g.Id == id);
            if (goalToDelete == null) return; // Safety check
            var response = await GoalService.DeleteGoal(goalToDelete);
            if (response.Success) // Assuming DeleteGoal returns true/false
            {
                Goals.Remove(goalToDelete); // Optimized: Remove item locally instead of refetching all goals
                StateHasChanged(); // Refresh UI
            }
            else
            {
                Message = response.Message;
            }
        }

        private string GetStatusBadge(GoalStatus status)
        {
            return status switch
            {
                GoalStatus.Completed => "<span class=\"badge bg-success rounded-pill\">Completed</span>",
                GoalStatus.InProgress => "<span class=\"badge bg-warning text-dark rounded-pill\">In Progress</span>",
                
                GoalStatus.OnHold => "<span class=\"badge bg-info text-dark rounded-pill\">On Hold</span>",
                GoalStatus.Cancelled => "<span class=\"badge bg-danger rounded-pill\">Cancelled</span>",
                _ => "<span class=\"badge bg-dark rounded-pill\">Unknown</span>"
            };
        }

        protected override async Task OnInitializedAsync()
        {
            Goals = await GoalService.GetGoals();
        }
    }
}

