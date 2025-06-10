using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models;

using GoalTracker.UI.Blazor.Models.ViewModels;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class Index : ComponentBase
    {
        [Inject] public IGoalService GoalService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        protected PagedResult<GoalViewModel>? pagedGoals;
        protected bool isLoading = false;
        protected string searchPhrase = "";
        protected int currentPage = 1;
        protected int pageSize = 10;
        protected string sortBy = "Title";
        protected int sortDirection = 0;
        protected string Message = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadGoals();
        }
     
        protected async Task LoadGoals()
        {
            isLoading = true;
            try
            {
                //; // totalRecords Get total count
                pagedGoals = await GoalService.GetGoals(searchPhrase, currentPage, pageSize, sortBy, sortDirection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
                Message = "Failed to load goals. Please try again.";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }


        private int totalRecords = 0; // Set this when you load your data
       
        protected async Task GoToPage(int page)
        {
            if (page >= 1 && page <= totalPages && page != currentPage)
            {
                currentPage = page;
                await LoadGoals();
            }
        }
        // Add this method to debug the values:
        private void DebugPagination()
        {
            Console.WriteLine($"totalRecords: {totalRecords}");
            Console.WriteLine($"pageSize: {pageSize}");
            Console.WriteLine($"currentPage: {currentPage}");
            Console.WriteLine($"totalPages: {totalPages}");
        }
        // Fixed totalPages calculation with proper validation:
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
            currentPage = 1; // Reset to first page when searching
            await LoadGoals();
        }

     

        protected async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newPageSize))
            {
                pageSize = newPageSize;
                currentPage = 1; // Reset to first page when changing page size
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
                // Toggle sort direction if same column
                sortDirection = sortDirection == 0 ? 1 : 0;
            }
            else
            {
                // New column, default to ascending
                sortBy = columnName;
                sortDirection = 0;
            }

            currentPage = 1; // Reset to first page when sorting
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

        protected async Task DeleteGoal(int goalId)
        {
            // You would typically show a confirmation dialog here
            try
            {
                // Implement delete functionality
                // await GoalService.DeleteGoal(goalId);
                // await LoadGoals(); // Refresh the list
                Message = "Delete functionality not implemented yet.";
            }
            catch (Exception ex)
            {
                Message = $"Error deleting goal: {ex.Message}";
            }
        }
    }
}