using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.Goals.Commands.CreateGoal;

public class CreateGoalCommand : IRequest<int>
{
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? TargetDate { get; set; }

    public GoalStatus? Status { get; set; }
    public Priority? Priority { get; set; }


}
