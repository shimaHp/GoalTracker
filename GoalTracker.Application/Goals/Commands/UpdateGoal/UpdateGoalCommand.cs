


using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.Goals.Commands.editGoal;

// For your update goal command
public class UpdateGoalCommand : IRequest<GoalDto>
{
    public UpdateGoalDto UpdateGoalDto { get; set; } = default!;
}