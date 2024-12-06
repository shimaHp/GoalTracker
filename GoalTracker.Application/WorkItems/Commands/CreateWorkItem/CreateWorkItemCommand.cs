

using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.WorkItems.Commands.CreateWorkItem;

public class CreateWorkItemCommand:IRequest<int>
{

    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public WorkItemStatus Status { get; set; }
    public int GoalId { get; set; }
   


}
