

using AutoMapper;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.WorkItems.Queries.GetWorkItemsForGoal;

public class GetWorkItemsForGoalQueryHandler(ILogger<GetWorkItemsForGoalQueryHandler> logger,IGoalsRepository goalsRepository,IMapper mapper) : IRequestHandler<GetWorkItemsForGoalQuery, IEnumerable<WorkItemDto>>
{
    public async Task<IEnumerable<WorkItemDto>> Handle(GetWorkItemsForGoalQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Retriving workitems by   GoalId with id:{request.GoalId}");
        var goal = await goalsRepository.GetGoalAsync(request.GoalId);
        if (goal == null) throw new NotFoundException(nameof(WorkItem), request.GoalId.ToString());

        var result= mapper.Map<IEnumerable<WorkItemDto>>(goal.WorkItems);
        return result;


    }
}
