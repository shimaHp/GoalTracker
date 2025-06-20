using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels.WorkItems;
using GoalTracker.UI.Blazor.Services.Base;
using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels.Goals
{
    public class UpdateGoalViewModel
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Goal title is required")]
        //[StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        //[StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string? Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public GoalStatus? Status { get; set; }

        public Priority? Priority { get; set; }

        // Work Items - combining existing, new, and tracking deletions
        public List<UpdateWorkItemViewModel> WorkItems { get; set; } = new();

        // Track deleted work item IDs for API call
        public List<int> DeletedWorkItemIds { get; set; } = new();

        // Helper methods to separate work items by type for the API
        public List<UpdateWorkItemViewModel> GetNewWorkItems()
        {
            return WorkItems.Where(wi => wi.Id == 0 && !wi.IsDeleted).ToList();
        }

        public List<UpdateWorkItemViewModel> GetExistingWorkItems()
        {
            return WorkItems.Where(wi => wi.Id > 0 && !wi.IsDeleted).ToList();
        }


    }
}