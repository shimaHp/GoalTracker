using Blazored.Toast.Services;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class CreateGoal
    {


        [Inject]  public NavigationManager Navigation { get; set; } = default!;
        [Inject] public IToastService ToastService { get; set; } = default!;
        [Inject] public IGoalService GoalService { get; set; } = default!;

        [Inject] IUserService UserService { get; set; } = default!;
        private List<CollaboratorViewModel> users = new();
        //[Inject] IUserService UserService { get; set; } = default!;
        //private List<UserViewModel> users = new();

        private CreateGoalViewModel goal = new();
        private bool isSubmitting = false;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                users = await UserService.GetCollaboratorsAsync(); // or GetUsersByRoleAsync("Collaborator")
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Failed to load users.");
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
        }
        private async Task HandleValidSubmit()
        {
            isSubmitting = true;
            //users = await UserService.GetAllUsersAsync();

            // 1. Manually validate nested WorkItems
            var context = new ValidationContext(goal);
            var results = new List<ValidationResult>();
            bool modelValid = Validator.TryValidateObject(goal, context, results, true);

            foreach (var workItem in goal.WorkItems)
            {
                var itemContext = new ValidationContext(workItem);
                var itemResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(workItem, itemContext, itemResults, true))
                {
                    modelValid = false;
                    results.AddRange(itemResults);
                }
            }

            if (!modelValid)
            {
                ToastService.ShowError("Please fix validation errors before submitting.");
                isSubmitting = false;
                return;
            }

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
            catch (Exception ex)
            {
                ToastService.ShowError("An unexpected error occurred.");
                Console.WriteLine($"[CreateGoal] Exception: {ex.Message}");
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