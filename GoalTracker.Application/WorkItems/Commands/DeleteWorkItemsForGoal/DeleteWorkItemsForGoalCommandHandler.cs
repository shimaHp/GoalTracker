

using AutoMapper;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;
using GoalTracker.Application.WorkItems.Commands.DeleteWorkItems;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.WorkItems.Commands.DeleteWorkItemsForGoal;

public class DeleteWorkItemsForGoalCommandHandler(ILogger<DeleteWorkItemsForGoalCommandHandler> logger,
    IMapper mapper,
    IGoalsRepository goalsRepository,
    IWorkItemRepository workItemRepository) : IRequestHandler<DeleteWorkItemsForGoalCommand>
{
    public async Task Handle(DeleteWorkItemsForGoalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting  WorkItems with goals Id:{goalId}" , request.GoalId);

        var goal = await goalsRepository.GetGoalAsync(request.GoalId);
        if (goal == null) throw new NotFoundException(nameof(goal), request.GoalId.ToString());

       

      await  workItemRepository.DeleteAsync(goal.WorkItems);
    }
}
