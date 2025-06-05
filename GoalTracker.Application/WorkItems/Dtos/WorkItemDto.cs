using GoalTracker.Domain.Enums;

namespace GoalTracker.Application.WorkItems.Dtos;

public class WorkItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public WorkItemStatus Status { get; set; }
    public int GoalId { get; set; }

    // Optionally for display
    public string? GoalTitle { get; set; }
    public string? GoalDescription { get; set; }

    // 👇 Add these to show user info in frontend
    public string CreatorId { get; set; }
    public string CreatorName { get; set; }
    public string CreatorEmail { get; set; }

    public string? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public string? AssigneeEmail { get; set; }

    public string? LastUpdatedById { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
}