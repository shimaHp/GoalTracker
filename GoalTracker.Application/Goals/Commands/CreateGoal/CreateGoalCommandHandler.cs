

using AutoMapper;
using GoalTracker.Application.Users;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.CreateGoal
{
    public class CreateGoalCommandHandler(ILogger<CreateGoalCommandHandler> logger, IMapper mapper, IGoalsRepository goalsRepository,IUserContext userContext ) : IRequestHandler<CreateGoalCommand, int>
    {
        public async Task<int> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var currentUser= userContext.GetCurrentUser();
            logger.LogInformation($"Creating a goal with title:{request.Title}");
            var goal = mapper.Map<Goal>(request);
            goal.UserId = currentUser.Id;
            int id = await goalsRepository.CreateAsync(goal);
            return id;
        }
    }
}
