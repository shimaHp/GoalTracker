
using AutoMapper;
using GoalTracker.Application.Common;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Goals.Queries.GetAllGoals;

public class GetAllGoalsQueryHandler(ILogger<GetAllGoalsQueryHandler> logger, IMapper mapper, IGoalsRepository goalsRepository) : IRequestHandler<GetAllGoalsQuery, PagedResult<GoalDto>>
{
    public async Task<PagedResult<GoalDto>> Handle(GetAllGoalsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting All Goals");
        var (goals, totalCount) = await goalsRepository.GetAllMatchingAsync(
            request.SearchPharse
            , request.PageSize
            , request.PageNumber
            , request.SortBy
            , request.sortDirection);

        var goalDTOs = mapper.Map<IEnumerable<GoalDto>>(goals);
       // var result = new PagedResult<GoalDto>(goalDTOs, goalDTOs.Count(), request.PageSize, request.PageNumber);
        var result = new PagedResult<GoalDto>(goalDTOs, totalCount, request.PageSize, request.PageNumber);

        return result!;
    }
}
