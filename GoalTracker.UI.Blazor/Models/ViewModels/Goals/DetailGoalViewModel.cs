using GoalTracker.UI.Blazor.Models.Enums;
using GoalTracker.UI.Blazor.Models.ViewModels.WorkItems;

namespace GoalTracker.UI.Blazor.Models.ViewModels.Goals
{
    public class DetailGoalViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? TargetDate { get; set; }
        public GoalStatus Status { get; set; }
        public Priority Priority { get; set; }
        public ICollection<DetailWorkItemViewModel> WorkItems { get; set; } = new List<DetailWorkItemViewModel>();

        // User-related details for display
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
