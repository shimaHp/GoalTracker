

using MediatR;

namespace GoalTracker.Application.Goals.Commands.DeleteGoal;

public class DeleteGoalCommand(int id):IRequest
{
    public int Id { get;  }=id;
}
