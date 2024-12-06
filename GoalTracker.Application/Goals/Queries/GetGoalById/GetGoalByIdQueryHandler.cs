

using AutoMapper;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Queries.GetGoalById;

public class GetGoalByIdQueryHandler(ILogger<GetGoalByIdQueryHandler> logger, IMapper mapper, IGoalsRepository goalsRepository) : IRequestHandler<GetGoalByIdQuery, GoalDto>
{
    public async Task<GoalDto> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting a Goal By Id:{request.Id}");
        var goal =  await goalsRepository.GetGoalAsync(request.Id) ?? 
            throw new NotFoundException(nameof(Goal), request.Id.ToString());
        
        var goalDto = mapper.Map<GoalDto>(goal);
        return goalDto;
    }
}
