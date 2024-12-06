
using MediatR;

namespace GoalTracker.Application.WorkItems.Commands.DeleteWorkItems;

public class DeleteWorkItemsForGoalCommand(int goalId):IRequest
{

    public int GoalId { get; } = goalId;
}
