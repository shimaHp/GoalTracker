using GoalTracker.Domain.Enums;

namespace GoalTracker.Application.WorkItems.Dtos;

public class WorkItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }
    public int? Status { get; set; }
    // Foreign key for the related Goal

}