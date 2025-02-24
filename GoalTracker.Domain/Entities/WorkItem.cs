using GoalTracker.Domain.Enums;

namespace GoalTracker.Domain.Entities;

public class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; }

    // Foreign key for the related Goal
    public int GoalId { get; set; }

    // Navigation property to the associated Goal
    public required Goal Goal { get; set; }

    //users
    public string CreatorId { get; set; } = default!;
    public User Creator { get; set; } = default!;

    // Who the work item is assigned to
    public string? AssigneeId { get; set; }
    public User? Assignee { get; set; }

    // Last person who updated the work item
    public string? LastUpdatedById { get; set; }
    public User? LastUpdatedBy { get; set; }

}
