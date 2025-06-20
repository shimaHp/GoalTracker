using GoalTracker.UI.Blazor.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels.WorkItems
{
    public class CreateWorkItemViewModel
    {


        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public WorkItemStatus Status { get; set; } = WorkItemStatus.NotStarted;

        //public string? AssigneeId { get; set; }

    }
}