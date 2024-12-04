


using MediatR;

namespace GoalTracker.Application.Goals.Commands.editGoal;

public class UpdateGoalCommand: IRequest<bool>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

  
    public DateTime? TargetDate { get; set; }

    public int? Status { get; set; }

    public int? Priority { get; set; }


}
