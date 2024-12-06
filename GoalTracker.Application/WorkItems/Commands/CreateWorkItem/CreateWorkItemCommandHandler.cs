

using AutoMapper;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.WorkItems.Commands.CreateWorkItem;

public class CreateWorkItemCommandHandler(
    ILogger<CreateWorkItemCommandHandler> logger,
    IMapper mapper,
    IGoalsRepository goalsRepository,
    IWorkItemRepository workItemRepository
    ) : IRequestHandler<CreateWorkItemCommand,int>
{
    public async Task<int> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new WorkItem: {@WorkItemRequest}", request);

        var goal= await goalsRepository.GetGoalAsync(request.GoalId);
        if (goal == null) throw new NotFoundException(nameof(goal),request.GoalId.ToString());

        var workItem = mapper.Map<WorkItem>(request);

   var wId= await workItemRepository.CreateAsync(workItem);

        return wId;


    }
}
