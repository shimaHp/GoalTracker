using System.ComponentModel.DataAnnotations;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    public class GoalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        //todo
        //[FutureDate(ErrorMessage = "Target date must be in the future")]
        public DateTime? TargetDate { get; set; }

        public GoalStatus Status { get; set; }

        public Priority Priority { get; set; }

        public ICollection<WorkItemViewModel> WorkItems { get; set; } = new List<WorkItemViewModel>();

        // UserId may be handled differently in the UI layer, possibly through auth context
        public string UserId { get; set; } = string.Empty;
    }
}
