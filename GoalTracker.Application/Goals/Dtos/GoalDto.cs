using GoalTracker.Application.WorkItems.Dtos;

namespace GoalTracker.Application.Goals.Dtos;

public class GoalDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? TargetDate { get; set; }
    public int? Status { get; set; }
    public int? Priority { get; set; }
    // Collection of WorkItems related to this Goal
    public ICollection<WorkItemDto> WorkItems { get; set; } = new List<WorkItemDto>();
}
