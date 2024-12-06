


using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.Goals.Commands.editGoal;

public class UpdateGoalCommand: IRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

  
    public DateTime? TargetDate { get; set; }

    public GoalStatus? Status { get; set; }
    public Priority? Priority { get; set; }


}
