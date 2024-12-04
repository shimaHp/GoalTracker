

using System.ComponentModel.DataAnnotations;
using GoalTracker.Application.Goals.WorkItems.Dtos;

namespace GoalTracker.Application.Goals.Dtos;

public class CreateGoalDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = default!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "CreatedDate is required.")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date, ErrorMessage = "TargetDate must be a valid date.")]
    public DateTime? TargetDate { get; set; }

    [Range(1, 5, ErrorMessage = "Status must be between 1 and 5.")]
    public int? Status { get; set; }

    [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
    public int? Priority { get; set; }

    //[Required(ErrorMessage = "WorkItems cannot be null.")]
    public ICollection<WorkItemDto?> WorkItems { get; set; } = new List<WorkItemDto>();
}
