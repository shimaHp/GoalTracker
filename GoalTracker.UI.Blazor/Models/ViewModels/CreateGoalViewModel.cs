using System.ComponentModel.DataAnnotations;
using static GoalTracker.UI.Blazor.Pages.Goals.CreateGoal;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    using GoalTracker.UI.Blazor.Models.Enums;
    using GoalTracker.UI.Blazor.Validators;
    using System.ComponentModel.DataAnnotations;

    public class CreateGoalViewModel
    {
        [Required(ErrorMessage = "Goal title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string? Description { get; set; }

        [FutureDateIfPresent(ErrorMessage = "Target date cannot be in the past.")]
        [Required(ErrorMessage = "Goal  DueDate is required")]
        public DateTime? TargetDate { get; set; }

        public GoalStatus? Status { get; set; }
        public Priority? Priority { get; set; }

        public List<CreateWorkItemViewModel> WorkItems { get; set; } = new();
      
    }

}
