using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? TargetDate { get; set; }
    public GoalStatus Status { get; set; }
    public Priority Priority { get; set; }
    // Collection of WorkItems related to this Goal
 
    public List<Application.WorkItems.Dtos.WorkItemDto> WorkItems { get; set; } = [];
}
