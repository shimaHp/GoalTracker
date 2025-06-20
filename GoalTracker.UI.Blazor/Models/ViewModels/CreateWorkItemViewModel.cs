using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Validators;
using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class CreateWorkItemViewModel
    {


        [Required(ErrorMessage = "Work item title is required")]
        [StringLength(200, ErrorMessage = "Work item title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Work item DueDate is required")]

        [FutureDateIfPresent(ErrorMessage = "Target date cannot be in the past.")]
        public DateTime? DueDate { get; set; }

        public WorkItemStatus Status { get; set; } = WorkItemStatus.NotStarted;

           public string? AssigneeId { get; set; }

    }
}