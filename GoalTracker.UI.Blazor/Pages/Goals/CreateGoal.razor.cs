using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Interfaces.Services;
using System.ComponentModel.DataAnnotations;
using Blazored.Toast.Services;
using System.Linq;
using GoalTracker.UI.Blazor.Models.ViewModels.UsersViewModel;
using GoalTracker.UI.Blazor.Models.ViewModels.Goals;
using GoalTracker.UI.Blazor.Models.ViewModels.WorkItems;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class CreateGoal : ComponentBase
    {
        [Inject] private IGoalService GoalService { get; set; }
        [Inject] private IUserService UserService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IToastService ToastService { get; set; }

        private CreateGoalViewModel goal = new();
        private EditContext editContext;
        private List<CollaboratorViewModel> users = new();
        private bool isSubmitting = false;
        private List<ValidationResult> customErrors = new();

        protected override async Task OnInitializedAsync()
        {
            // Initialize EditContext
            editContext = new EditContext(goal);

            // Initialize goal with default values
            goal.Priority = Priority.Medium;
            goal.Status = GoalStatus.NotStarted;
            goal.WorkItems = new List<CreateWorkItemViewModel>();

            // Load users for work item assignees
            await LoadUsers();
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

        private void AddWorkItem()
        {
            var newWorkItem = new CreateWorkItemViewModel
            {
                Status = WorkItemStatus.NotStarted,
                DueDate = DateTime.Today.AddDays(7) // Default to 7 days from now
            };

            goal.WorkItems.Add(newWorkItem);

            // Notify EditContext of changes
            editContext?.NotifyValidationStateChanged();
            StateHasChanged();
        }

        private void RemoveWorkItem(int index)
        {
            if (index >= 0 && index < goal.WorkItems.Count)
            {
                goal.WorkItems.RemoveAt(index);

                // Notify EditContext of changes
                editContext?.NotifyValidationStateChanged();
                StateHasChanged();
            }
        }

        private void NotifyFieldChanged()
        {
            // This method will be called when work item fields change
            editContext?.NotifyValidationStateChanged();
        }

        private bool IsFormValid()
        {
            var validationContext = new ValidationContext(goal, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            // Validate the main goal object
            bool isValid = Validator.TryValidateObject(goal, validationContext, validationResults, validateAllProperties: true);

            // Validate each work item individually
            if (goal.WorkItems != null && goal.WorkItems.Any())
            {
                for (int i = 0; i < goal.WorkItems.Count; i++)
                {
                    var workItem = goal.WorkItems[i];
                    var workItemContext = new ValidationContext(workItem, serviceProvider: null, items: null);
                    var workItemResults = new List<ValidationResult>();

                    bool workItemValid = Validator.TryValidateObject(workItem, workItemContext, workItemResults, validateAllProperties: true);

                    if (!workItemValid)
                    {
                        isValid = false;

                        // Prefix work item validation errors with work item number
                        foreach (var result in workItemResults)
                        {
                            var prefixedError = new ValidationResult(
                                $"Work Item {i + 1}: {result.ErrorMessage}",
                                result.MemberNames
                            );
                            validationResults.Add(prefixedError);
                        }
                    }
                }
            }

            // Update custom errors for display
            customErrors = validationResults;

            return isValid;
        }

        private async Task HandleValidSubmit()
        {
            // Double-check validation before submitting
            if (!IsFormValid())
            {
                ToastService.ShowWarning("Please fix the validation errors before submitting");
                editContext?.NotifyValidationStateChanged();
                StateHasChanged();
                return;
            }

            // Clear any previous custom errors
            customErrors.Clear();

            try
            {
                isSubmitting = true;
                StateHasChanged();

                var response = await GoalService.CreateGoal(goal);

                if (response.Success)
                {
                    ToastService.ShowSuccess("Goal created successfully!");
                    // Navigate back to goals list or goal detail
                    NavigationManager.NavigateTo("/goals");
                }
                else
                {
                    // Handle API validation errors
                    if (!string.IsNullOrEmpty(response.ValidationErrors))
                    {
                        customErrors = new List<ValidationResult>
                        {
                            new ValidationResult(response.ValidationErrors)
                        };
                        ToastService.ShowError("Please fix the validation errors");
                    }
                    else if (!string.IsNullOrEmpty(response.Message))
                    {
                        customErrors = new List<ValidationResult>
                        {
                            new ValidationResult(response.Message)
                        };
                        ToastService.ShowError(response.Message);
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating goal: {ex.Message}");
                ToastService.ShowError("An unexpected error occurred while creating the goal. Please try again.");
                customErrors = new List<ValidationResult>
                {
                    new ValidationResult("An unexpected error occurred while creating the goal. Please try again.")
                };
                StateHasChanged();
            }
            finally
            {
                isSubmitting = false;
                StateHasChanged();
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/goals");
        }

        // Override OnAfterRender to ensure EditContext is properly initialized
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && editContext == null)
            {
                editContext = new EditContext(goal);
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);
        }

        // Helper method to validate work item on field change
        private void ValidateWorkItem(int index)
        {
            if (index >= 0 && index < goal.WorkItems.Count)
            {
                var workItem = goal.WorkItems[index];
                var workItemContext = new ValidationContext(workItem);
                var workItemResults = new List<ValidationResult>();

                Validator.TryValidateObject(workItem, workItemContext, workItemResults, validateAllProperties: true);

                // Update custom errors for this specific work item
                customErrors.RemoveAll(ce => ce.ErrorMessage.StartsWith($"Work Item {index + 1}:"));

                foreach (var result in workItemResults)
                {
                    var prefixedError = new ValidationResult(
                        $"Work Item {index + 1}: {result.ErrorMessage}",
                        result.MemberNames
                    );
                    customErrors.Add(prefixedError);
                }

                editContext?.NotifyValidationStateChanged();
                StateHasChanged();
            }
        }

        // Method to handle work item field changes with validation
        private void OnWorkItemFieldChanged(int index)
        {
            ValidateWorkItem(index);
        }
    }
}