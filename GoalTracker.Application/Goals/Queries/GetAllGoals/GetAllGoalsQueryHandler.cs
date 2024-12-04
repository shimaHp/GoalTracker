
using AutoMapper;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Queries.GetAllGoals;

public class GetAllGoalsQueryHandler(ILogger<GetAllGoalsQueryHandler> logger,IMapper mapper,IGoalsRepository goalsRepository) : IRequestHandler<GetAllGoalsQuery, IEnumerable<GoalDto>>
{
    public async Task<IEnumerable<GoalDto>> Handle(GetAllGoalsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting All Goals");
        var goals = await goalsRepository.GetAllAsync();

        var goalDtos = mapper.Map<IEnumerable<GoalDto>>(goals);
        return goalDtos;
    }
}
