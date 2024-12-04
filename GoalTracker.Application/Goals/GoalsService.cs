

using AutoMapper;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals;

internal class GoalsService(IGoalsRepository goalsRepository, ILogger<GoalsService> logger, IMapper mapper) : IGoalsService
{

    public async Task<IEnumerable<GoalDto>> GetAllGoals()
    {
        logger.LogInformation("Getting All Goals");
        var goal = await goalsRepository.GetAllAsync();

        var goalDto = mapper.Map<IEnumerable<GoalDto>>(goal);
        return goalDto;
    }

    public async Task<GoalDto?> GetGoalById(int GId)
    {
        logger.LogInformation($"Getting a Goal By Id:{GId}");
        var goal = await goalsRepository.GetGoalAsync(GId);
        var goalDto = mapper.Map<GoalDto?>(goal);
        return goalDto;
    }

    public async Task<int> CreateGoal(CreateGoalDto dto)
    {
        logger.LogInformation($"Creating a goal with title:{dto.Title}");
        var goal= mapper.Map<Goal>(dto);
        int id=await goalsRepository.CreateAsync(goal);
        return id;

    }
}
