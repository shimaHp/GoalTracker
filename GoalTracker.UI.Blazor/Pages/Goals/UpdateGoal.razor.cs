using Blazored.Toast.Services;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels;
using GoalTracker.UI.Blazor.Models.ViewModels.Goals;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Models.ViewModels.WorkItems;
using GoalTracker.UI.Blazor.Services;
using GoalTracker.UI.Blazor.Services.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class UpdateGoal:ComponentBase
    {
        [Inject] public IGoalService GoalService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] private IUserService UserService { get; set; }
        [Parameter] public int Id { get; set; }
        [Inject] private IToastService ToastService { get; set; }

        private UpdateGoalViewModel? Model { get; set; }
        private bool IsLoading { get; set; } = true;
        private bool IsSubmitting { get; set; } = false;
        private string? ErrorMessage { get; set; }
        private List<CollaboratorViewModel> users = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();

            await LoadGoal();
        }

        private async Task LoadGoal()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                // Load the existing goal
                var goalViewModel = await GoalService.GetGoalDetail(Id);

                if (goalViewModel != null)
                {
                    // Map the GoalViewModel to UpdateGoalViewModel
                    Model = new UpdateGoalViewModel
                    {
                        Id = goalViewModel.Id,
                        Title = goalViewModel.Title,
                        Description = goalViewModel.Description,
                        TargetDate = goalViewModel.TargetDate?.DateTime, // Convert DateTimeOffset to DateTime
                        Status = goalViewModel.Status,
                        Priority = goalViewModel.Priority,
                        WorkItems = goalViewModel.WorkItems?.Select(wi => new UpdateWorkItemViewModel
                        {
                            Id = wi.Id,
                            Title = wi.Title,
                            Description = wi.Description,
                            DueDate = wi.DueDate?.DateTime, // Already DateTime?
                            Status = wi.Status,
                            IsDeleted = false,
                            AssigneeId=wi.AssigneeId

                        }).ToList() ?? new List<UpdateWorkItemViewModel>()
                    };
                }
                else
                {
                    ErrorMessage = "Goal not found or failed to load";
                    Model = null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while loading the goal";
                Model = null;
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task LoadUsers()
        {
            try
            {
                users = await UserService.GetCollaboratorsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                ToastService.ShowError("Failed to load users for assignment");
                users = new List<CollaboratorViewModel>();
            }
        }

        private async Task HandleValidSubmit()
        {
            if (Model == null) return;

            try
            {
                IsSubmitting = true;
                ErrorMessage = null;

                var response = await GoalService.UpdateGoal(Id, Model);

                if (response.Success && response.Data != null)
                {
                    // Optionally cache the updated goal if you want
                     GoalService.CachedGoal = response.Data;

                    Navigation.NavigateTo($"/goals/{response.Data.Id}");
                }
                else
                {
                    ErrorMessage = response.Message ?? "Failed to update goal";

                    if (response.ValidationErrors != null && response.ValidationErrors.Any())
                    {
                        ErrorMessage += "\n" + string.Join("\n", response.ValidationErrors);
                    }
                }
            }
            catch (Exception)
            {
                ErrorMessage = "An unexpected error occurred while updating the goal";
            }
            finally
            {
                IsSubmitting = false;
            }
        }


        private void AddNewWorkItem()
        {
            if (Model == null) return;

            Model.WorkItems.Add(new UpdateWorkItemViewModel
            {
                Id = 0, // 0 indicates a new item
                Title = string.Empty,
                Description = null,
                DueDate = null,
                Status = WorkItemStatus.NotStarted,
                IsDeleted = false
            });

            StateHasChanged();
        }

        private void RemoveWorkItem(int index)
        {
            if (Model == null || index < 0 || index >= Model.WorkItems.Count) return;

            var workItem = Model.WorkItems[index];

            if (workItem.IsNew)
            {
                // If it's a new item, just remove it from the list
                Model.WorkItems.RemoveAt(index);
            }
            else
            {
                // If it's an existing item, mark it as deleted and add to deleted IDs
                workItem.IsDeleted = true;
                if (!Model.DeletedWorkItemIds.Contains(workItem.Id))
                {
                    Model.DeletedWorkItemIds.Add(workItem.Id);
                }
            }

            StateHasChanged();
        }

        private void GoBack()
        {
            Navigation.NavigateTo("/goals");
        }

        // Optional: Add confirmation dialog for navigation away with unsaved changes
        private async Task<bool> ConfirmNavigation()
        {
            //todo
            return true;// await JSRuntime.InvokeAsync<bool>("confirm",         "You have unsaved changes. Are you sure you want to leave this page?");
        }

        // Optional: Method to reset form to original state
        private async Task ResetForm()
        {
            //todo
            //if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to reset all changes?"))
            //{
            //    await LoadGoal();
            //}
            await LoadGoal();
        }

        // Optional: Method to save as draft (if you implement draft functionality)
        private async Task SaveDraft()
        {
            // Implementation for saving draft
            // This could call a separate API endpoint for drafts
        }
    }
}