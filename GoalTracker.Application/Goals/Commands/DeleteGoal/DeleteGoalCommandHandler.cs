

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.DeleteGoal;

public class DeleteGoalCommandHandler(ILogger<DeleteGoalCommandHandler> logger,  IGoalsRepository goalsRepository) : IRequestHandler<DeleteGoalCommand,bool>
{
    public async Task<bool> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleteing  Goal with id:{request.Id}");
        var goal =  await goalsRepository.GetGoalAsync(request.Id);
        if (goal is null)
            return false;

        await goalsRepository.DeleteAsync(goal);
        return true;

    }
}
