using System.ComponentModel.DataAnnotations;
using static GoalTracker.UI.Blazor.Pages.Goals.CreateGoal;

namespace GoalTracker.UI.Blazor.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGoalViewModel
    {
        [Required(ErrorMessage = "Goal title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string? Description { get; set; }

        public DateTime? TargetDate { get; set; }

        public GoalStatus? Status { get; set; }
        public Priority? Priority { get; set; }

        public List<CreateWorkItemViewModel> WorkItems { get; set; } = new();


      

      
    }

}
