

using AutoMapper;
using GoalTracker.Application.Users;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Commands.CreateGoal
{
    public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<CreateGoalCommandHandler> _logger;

        public CreateGoalCommandHandler(
            ILogger<CreateGoalCommandHandler> logger,
            IMapper mapper,
            IGoalsRepository goalsRepository,
            IUserContext userContext)
        {
            _logger = logger;
            _mapper = mapper;
            _goalsRepository = goalsRepository;
            _userContext = userContext;
        }

        public async Task<int> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            _logger.LogInformation("User {UserId} is creating goal '{Title}' with {WorkItemCount} work items",
    currentUser.Id, request.Title, request.WorkItems?.Count ?? 0);



            // Map command to domain Goal
            var goal = _mapper.Map<Goal>(request);

            // Set properties manually
            goal.UserId = currentUser.Id;
            goal.CreatedDate = DateTime.UtcNow;

            //Todo: check in debuging agian
            if (request.WorkItems?.Any() == true)
            {
                goal.WorkItems = _mapper.Map<List<WorkItem>>(request.WorkItems);

                foreach (var workItem in goal.WorkItems)
                {
                    workItem.CreatorId = currentUser.Id;
                    workItem.Goal = goal; // navigation property
                    workItem.CreatedDate = DateTime.UtcNow;
                }              
            }
            int goalId = await _goalsRepository.CreateAsync(goal);
            _logger.LogInformation(
              "Successfully created goal with ID {GoalId}", goalId);
            return goalId;
        }
    }
}