

using GoalTracker.Domain.Enums;
using MediatR;

namespace GoalTracker.Application.WorkItems.Commands.CreateWorkItem;

public class CreateWorkItemCommand:IRequest<int>
{

    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public WorkItemStatus? Status { get; set; }




}
