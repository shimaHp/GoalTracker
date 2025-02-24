

using MediatR;

namespace GoalTracker.Application.WorkItems.Commands.UpdateWorkItem;

public class UpdateWorkItemCommand(int workItemId) : IRequest
{

    public int WorkItemId { get; } = workItemId;
}
