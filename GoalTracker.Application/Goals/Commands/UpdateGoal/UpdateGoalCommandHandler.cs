

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandHandler(ILogger<UpdateGoalCommandHandler> logger, IMapper mapper, IGoalsRepository goalsRepository) : IRequestHandler<UpdateGoalCommand, bool>
{
    public async Task<bool> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updating  Goal with id:{request.Id}");
        var goal = await goalsRepository.GetGoalAsync(request.Id);
        if (goal is null)
            return false;


        mapper.Map(request,goal);
        await goalsRepository.SaveChanges();
        return true;
    }
}
