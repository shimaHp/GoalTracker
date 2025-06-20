using GoalTracker.Application.Users.Dtos;
using MediatR;


namespace GoalTracker.Application.Users.Queries.GetUsersInRoleQuery
{
  

    public record GetUsersInRoleQuery(string Role) : IRequest<List<UserDto>>;
}
