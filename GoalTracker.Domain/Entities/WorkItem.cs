using GoalTracker.Domain.Enums;

namespace GoalTracker.Domain.Entities;

public class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } =DateTime.Now;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; }
 
    // Foreign key for the related Goal
    public int GoalId { get; set; }

    // Navigation property to the associated Goal
    public required Goal Goal { get; set; }
}
