using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class CreateGoal
    {


        [Inject]
        public NavigationManager Navigation { get; set; } = default!;


        [Inject]
        public IGoalService GoalService { get; set; } = default!;

        private CreateGoalViewModel goal = new();
        private bool isSubmitting = false;

        private async Task HandleValidSubmit()
        {
            isSubmitting = true;

            try
            {
                var result = await GoalService.CreateGoal(goal);
                if (result.Success)
                {
                    ToastService.ShowSuccess("Goal created successfully!");
                    Navigation.NavigateTo("/goals");
                }
                else
                {
                    ToastService.ShowError(result.Message ?? "Failed to create goal.");
                }
            }
            finally
            {
                isSubmitting = false;
            }
        }
        private void AddWorkItem()
        {
            if (goal.WorkItems == null)
                goal.WorkItems = new List<CreateWorkItemViewModel>();

            goal.WorkItems.Add(new CreateWorkItemViewModel());
        }

        private void RemoveWorkItem(int index)
        {
            if (goal.WorkItems != null && index >= 0 && index < goal.WorkItems.Count)
            {
                goal.WorkItems.RemoveAt(index);
            }
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/goals");
        }
    }
}