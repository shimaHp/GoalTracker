using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class UpdateWorkItemViewModel
    {
        public int Id { get; set; } // 0 for new items, > 0 for existing items

        [Required(ErrorMessage = "Work item title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public WorkItemStatus? Status { get; set; }

        // UI state properties
        public bool IsDeleted { get; set; } = false;

        // Helper property to identify new items
        public bool IsNew => Id == 0;
    }
}
