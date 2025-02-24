

using MediatR;

namespace GoalTracker.Application.WorkItems.Commands.UpdateWorkItem;

public class UpdateWorkItemCommandHandle : IRequestHandler<UpdateWorkItemCommand>
{
    public Task Handle(UpdateWorkItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
