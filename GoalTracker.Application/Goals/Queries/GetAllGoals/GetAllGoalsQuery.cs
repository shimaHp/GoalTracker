

using GoalTracker.Application.Goals.Dtos;
using MediatR;

namespace GoalTracker.Application.Goals.Queries.GetAllGoals;

public class GetAllGoalsQuery:IRequest<IEnumerable<GoalDto>>
{

}
