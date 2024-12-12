using MediatR;

namespace GoalTracker.Application.Users.Commands.UpdateUserDetails;


public class UpdateUserDetailsCommand : IRequest
{
    public DateOnly? DateOfBirth { get; set; }

}