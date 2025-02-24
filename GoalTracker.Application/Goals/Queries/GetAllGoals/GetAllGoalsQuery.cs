

using GoalTracker.Application.Common;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Constants;
using MediatR;

namespace GoalTracker.Application.Goals.Queries.GetAllGoals;

public class GetAllGoalsQuery:IRequest<PagedResult<GoalDto>>
{
    public string? SearchPharse { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public string? SortBy { get; set; }
    public SortDirection sortDirection { get; set; }
}
