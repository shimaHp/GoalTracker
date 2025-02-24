

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.DeleteGoal;

public class DeleteGoalCommandHandler(ILogger<DeleteGoalCommandHandler> logger,
    IGoalsRepository goalsRepository,
    IGoalAuthorizationService goalAuthorizationService
    ) : IRequestHandler<DeleteGoalCommand>
{
    public async Task Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleteing  Goal with id:{request.Id}");
        var goal =  await goalsRepository.GetGoalAsync(request.Id);
        if (goal is null)
            throw new NotFoundException(nameof(Goal), request.Id.ToString());
        if (!goalAuthorizationService.Authorize(goal, ResourceOperation.Delete))
            throw new ForbidException();
        await goalsRepository.DeleteAsync(goal);
       

    }
}
