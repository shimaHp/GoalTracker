

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandHandler(ILogger<UpdateGoalCommandHandler> logger, IMapper mapper, IGoalsRepository goalsRepository) : IRequestHandler<UpdateGoalCommand>
{
    public async Task Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating  Goal with id:{request.Id}");
        var goal = await goalsRepository.GetGoalAsync(request.Id);
        if (goal is null)
            throw new NotFoundException(nameof(Goal), request.Id.ToString());



        mapper.Map(request,goal);
        await goalsRepository.SaveChanges();
      
    }
}
