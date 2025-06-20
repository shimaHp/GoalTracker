using GoalTracker.Application.Common.Interfaces;
using GoalTracker.Application.Users.Commands.AssignUserRole;
using GoalTracker.Application.Users.Commands.Login;
using GoalTracker.Application.Users.Commands.UnassignUserRole;
using GoalTracker.Application.Users.Commands.UpdateUserDetails;
using GoalTracker.Application.Users.Dtos;
using GoalTracker.Application.Users.Queries.GetUsersInRoleQuery;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;
using GoalTracker.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace GoalTracker.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(
    IMediator mediator,
    SignInManager<User> signInManager,
    IJwtService jwtService) : ControllerBase
{

  
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }


    [HttpPatch("user")]

    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]

    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }


    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]

    public async Task<IActionResult> UnassignUserRole(UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(UserManager<User> userManager) : ControllerBase
    {
        [HttpGet("in-role/{roleName}")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var users = await userManager.GetUsersInRoleAsync(roleName);
            return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
        }
    }

  
    [HttpGet("byrole/{roleName}")]
    public async Task<ActionResult<List<CollaboratorDto>>> GetUsersByRole(string roleName)
    {
        var users = await mediator.Send(new GetUsersInRoleQuery(roleName));
        var result = users.Select(u => new CollaboratorDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email
        }).ToList();
        return Ok(result);
    }
}
