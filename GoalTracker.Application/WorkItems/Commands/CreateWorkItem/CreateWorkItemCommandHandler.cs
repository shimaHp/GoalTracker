

using AutoMapper;
using GoalTracker.Application.Users;
using GoalTracker.Domain;
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
    IWorkItemRepository workItemRepository,
    IUserContext userContext,
    IGoalAuthorizationService goalAuthorizationService
    ) : IRequestHandler<CreateWorkItemCommand,int>
{
    public async Task<int> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Creating new WorkItem: {@WorkItemRequest}", request);


        var goal= await goalsRepository.GetGoalAsync(1);
        if (goal == null) throw new NotFoundException(nameof(goal),1.ToString());
        if (!goalAuthorizationService.Authorize(goal, ResourceOperation.Update))
            throw new ForbidException();

        var workItem = mapper.Map<WorkItem>(request);
        workItem.CreatorId = currentUser.Id;

   var wId= await workItemRepository.CreateAsync(workItem);

        return wId;


    }
}
