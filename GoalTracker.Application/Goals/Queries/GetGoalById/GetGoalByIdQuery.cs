

using GoalTracker.Application.Goals.Dtos;
using MediatR;

namespace GoalTracker.Application.Goals.Queries.GetGoalById;

public class GetGoalByIdQuery(int id):IRequest<GoalDto>
{
    public int Id { get; } = id;
}
