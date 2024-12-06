

using GoalTracker.Application.WorkItems.Dtos;
using MediatR;

namespace GoalTracker.Application.WorkItems.Queries.GetWorkItemByIdForGoal;

public class GetWorkItemByIdForGoalQuery(int goalId,int workItemId):IRequest<WorkItemDto>
{
    public int GoalId { get; } = goalId;

    public int WorkItemId { get; } = workItemId;
}
