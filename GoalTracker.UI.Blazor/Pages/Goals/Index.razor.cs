using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
            NavigationManager.NavigateTo("/Goal/create");
        }
        protected void UpdateGoal(int id)
        {
            NavigationManager.NavigateTo($"/Goal/upadete/{id}");
        }
        protected void GetGoalDetail(int id)
        {
            NavigationManager.NavigateTo($"/Goal/GoalDetails/{id}");
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


        
        protected override async Task OnInitializedAsync()
        {
            Goals = await GoalService.GetGoals();
        }
    }
}

