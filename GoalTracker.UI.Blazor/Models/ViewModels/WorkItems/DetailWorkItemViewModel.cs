using GoalTracker.UI.Blazor.Models.Enums;

namespace GoalTracker.UI.Blazor.Models.ViewModels.WorkItems
{
    public class DetailWorkItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public WorkItemStatus Status { get; set; }

        // User-related details for display
        //CreatedDate
        public DateTimeOffset? CreatedDate { get; set; }

        public string CreatorId { get; set; } = string.Empty;
        public string? CreatorEmail { get; set; }

        public string? AssigneeId { get; set; }
        public string? AssigneeEmail { get; set; }

        public string? LastUpdatedById { get; set; }
        public string? LastUpdatedByName { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
    }
}
