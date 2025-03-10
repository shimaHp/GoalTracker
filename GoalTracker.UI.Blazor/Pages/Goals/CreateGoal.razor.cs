using GoalTracker.UI.Blazor.Interfaces.Services;
using GoalTracker.UI.Blazor.Models.ViewModels;
using Microsoft.AspNetCore.Components;

namespace GoalTracker.UI.Blazor.Pages.Goals
{
    public partial class CreateGoal
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IGoalService GoalService { get; set; }

        private CreateGoalViewModel goalModel = new CreateGoalViewModel();
        //todo
        //private async Task HandleValidSubmit()
        //{
        //    try
        //    {
        //        // Create the goal command with current date
        //        var createGoalCommand = new CreateGoalCommand
        //        {
        //            Title = goalModel.Title,
        //            Description = goalModel.Description,
        //            CreatedDate = DateTime.Now,
        //            TargetDate = goalModel.TargetDate,
        //            Status = goalModel.Status ?? GoalStatus.NotStarted,
        //            Priority = goalModel.Priority ?? Priority.Medium,
        //            WorkItems = goalModel.WorkItems.Select(wi => new CreateWorkItemCommand
        //            {
        //                Title = wi.Title,
        //                Status = wi.Status ?? WorkItemStatus.InProgress,
        //                Priority = wi.Priority ?? Priority.Medium
        //            }).ToList()
        //        };

        //        // Call service method to create goal
        //        var newGoalId = await GoalService.CreateGoal(createGoalCommand);

        //        // Navigate back to goals list or goal details
        //        NavigationManager.NavigateTo($"/Goal/GoalDetails/{newGoalId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors (you might want to add error display logic)
        //        Console.WriteLine($"Error creating goal: {ex.Message}");
        //    }
        //}

        private void AddWorkItem()
        {
            goalModel.WorkItems.Add(new CreateWorkItemViewModel());
        }

        private void RemoveWorkItem(int index)
        {
            if (index >= 0 && index < goalModel.WorkItems.Count)
            {
                goalModel.WorkItems.RemoveAt(index);
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/Goals");
        }
    }

    // ViewModel for Create Goal
    public class CreateGoalViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? TargetDate { get; set; }
        public GoalStatus? Status { get; set; }
        public Priority? Priority { get; set; }
        public List<CreateWorkItemViewModel> WorkItems { get; set; } = new List<CreateWorkItemViewModel>();
    }

    // ViewModel for Create Work Item
    public class CreateWorkItemViewModel
    {
        public string Title { get; set; } = string.Empty;
        public WorkItemStatus? Status { get; set; }
        public Priority? Priority { get; set; }
    }
}