

using AutoMapper;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Application.WorkItems.Queries.GetWorkItemsForGoal;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.WorkItems.Queries.GetWorkItemByIdForGoal;

public class GetWorkItemByIdForGoalQueryHandler(ILogger<GetWorkItemByIdForGoalQueryHandler> logger, IGoalsRepository goalsRepository, IMapper mapper) : IRequestHandler<GetWorkItemByIdForGoalQuery, WorkItemDto>
{
    public async Task<WorkItemDto> Handle(GetWorkItemByIdForGoalQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retriving workitem:{WorkItemId}, for Goal with id:{GoalId}",request.GoalId,request.WorkItemId);

        var goal = await goalsRepository.GetGoalAsync(request.GoalId);
        if (goal == null) throw new NotFoundException(nameof(WorkItem), request.WorkItemId.ToString());

        var workItem=goal.WorkItems.FirstOrDefault(w=>w.Id==request.WorkItemId);

        if (workItem == null) throw new NotFoundException(nameof(workItem), request.WorkItemId.ToString());
        var result=mapper.Map<WorkItemDto>(workItem);
        return result;
    }
}
