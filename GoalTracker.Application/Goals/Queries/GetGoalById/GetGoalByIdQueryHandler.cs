

using AutoMapper;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Queries.GetGoalById;

public class GetGoalByIdQueryHandler(ILogger<GetGoalByIdQueryHandler> logger, IMapper mapper, IGoalsRepository goalsRepository) : IRequestHandler<GetGoalByIdQuery, GoalDto?>
{
    public async Task<GoalDto?> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting a Goal By Id:{request.Id}");
        var goal =  await goalsRepository.GetGoalAsync(request.Id);
        var goalDto = mapper.Map<GoalDto?>(goal);
        return goalDto;
    }
}
