using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class WorkItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        //todo

       // [FutureDate(ErrorMessage = "Due date must be in the future")]
        public DateTime? DueDate { get; set; }

        public WorkItemStatus Status { get; set; }

        [Required(ErrorMessage = "Goal is required")]
        public int GoalId { get; set; }

        // In the UI, we might include just the title of the goal, not the whole object
        public string? GoalTitle { get; set; }

        // Creator information
        public string CreatorId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;

        // Assignee information
        public string? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }

        // Last updated information
        public string? LastUpdatedById { get; set; }
        public string? LastUpdatedByName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
