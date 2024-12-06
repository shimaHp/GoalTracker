

using GoalTracker.Application.WorkItems.Dtos;
using MediatR;

namespace GoalTracker.Application.WorkItems.Queries.GetWorkItemsForGoal;

public class GetWorkItemsForGoalQuery(int goalId): IRequest<IEnumerable<WorkItemDto>>
{
    public int GoalId { get; } = goalId;
}
