using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models;
using GoalTracker.UI.Blazor.Models.ViewModels;
using Blazored.Toast.Services; // Add this

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class Index : ComponentBase
    {
        [Inject] public IGoalService GoalService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] public IToastService ToastService { get; set; } = default!; // Add this

        protected PagedResult<GoalViewModel>? pagedGoals;
        protected bool isLoading = false;
        protected string searchPhrase = "";
        protected int currentPage = 1;
        protected int pageSize = 10;
        protected string sortBy = "Title";
        protected int sortDirection = 0;
        protected string Message = "";

        // Delete confirmation variables
        private bool showConfirmDelete = false;
        private int goalIdToDelete;
        private string goalTitleToDelete = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadGoals();
        }

        protected async Task LoadGoals()
        {
            isLoading = true;
            try
            {
                pagedGoals = await GoalService.GetGoals(searchPhrase, currentPage, pageSize, sortBy, sortDirection);
                totalRecords = pagedGoals?.TotalCount ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
                ToastService.ShowError("Failed to load goals. Please try again.");
                totalRecords = 0;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private int totalRecords = 0;

        protected async Task GoToPage(int page)
        {
            if (page >= 1 && page <= totalPages && page != currentPage)
            {
                currentPage = page;
                await LoadGoals();
            }
        }

        private int totalPages
        {
            get
            {
                if (totalRecords <= 0 || pageSize <= 0)
                    return 1;
                return (int)Math.Ceiling((double)totalRecords / pageSize);
            }
        }

        protected async Task SearchGoals()
        {
            currentPage = 1;
            await LoadGoals();
        }

        protected async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newPageSize))
            {
                pageSize = newPageSize;
                currentPage = 1;
                await LoadGoals();
            }
        }

        protected async Task OnSearchKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SearchGoals();
            }
        }

        protected void CreateGoal()
        {
            Navigation.NavigateTo("/goals/create");
        }

        protected async Task SortBy(string columnName)
        {
            if (sortBy == columnName)
            {
                sortDirection = sortDirection == 0 ? 1 : 0;
            }
            else
            {
                sortBy = columnName;
                sortDirection = 0;
            }
            currentPage = 1;
            await LoadGoals();
        }

        protected string GetStatusColor(GoalStatus status)
        {
            return status switch
            {
                GoalStatus.Completed => "success",
                GoalStatus.InProgress => "warning",
                GoalStatus.NotStarted => "secondary",
                GoalStatus.Cancelled => "danger",
                _ => "secondary"
            };
        }

        protected void ViewGoal(int goalId)
        {
            Navigation.NavigateTo($"/goals/{goalId}");
        }

        protected void EditGoal(int goalId)
        {
            Navigation.NavigateTo($"/goals/edit/{goalId}");
        }

        // Updated delete methods with Toast notifications
        protected void ConfirmDeleteGoal(int goalId, string goalTitle)
        {
            goalIdToDelete = goalId;
            goalTitleToDelete = goalTitle;
            showConfirmDelete = true;
        }

        protected async Task DeleteGoalConfirmed()
        {
            try
            {
                showConfirmDelete = false; // Hide dialog first

                // Show loading toast
                ToastService.ShowInfo($"Deleting goal '{goalTitleToDelete}'...");

                await GoalService.DeleteGoal(goalIdToDelete);
                await LoadGoals(); // Refresh list

                // Show success toast
                ToastService.ShowSuccess($"Goal '{goalTitleToDelete}' deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting goal: {ex.Message}");
                ToastService.ShowError($"Failed to delete goal. Error: {ex.Message}");
            }
        }

        protected void CancelDelete()
        {
            showConfirmDelete = false;
            ToastService.ShowInfo("Delete operation cancelled.");
        }
    }
}