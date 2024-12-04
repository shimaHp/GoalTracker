

using MediatR;

namespace GoalTracker.Application.Goals.Commands.DeleteGoal;

public class DeleteGoalCommand(int id):IRequest<bool>
{
    public int Id { get;  }=id;
}
