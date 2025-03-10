using GoalTracker.Application.Common.Interfaces;
using GoalTracker.Application.Users.Commands.AssignUserRole;
using GoalTracker.Application.Users.Commands.Login;
using GoalTracker.Application.Users.Commands.UnassignUserRole;
using GoalTracker.Application.Users.Commands.UpdateUserDetails;
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
}
