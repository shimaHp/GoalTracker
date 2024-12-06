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


}