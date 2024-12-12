using System.Reflection.Metadata.Ecma335;
using GoalTracker.Application.Goals;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.DeleteGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.Goals.Queries.GetAllGoals;
using GoalTracker.Application.Goals.Queries.GetGoalById;
using GoalTracker.Domain.Constants;
using GoalTracker.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTracker.API.Controllers;


[ApiController]
[Route("api/goals")]
[Authorize]
public class GoalsController(IMediator mediator ) : ControllerBase
{
    [HttpGet]
    [Authorize(PolicyNames.AtLeast18)]
    public async Task<IActionResult> GetAll()
    {
        var goals= await mediator.Send(new GetAllGoalsQuery()); 
        return Ok(goals);
            
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoalById([FromRoute] int id)
    {
        var goal = await mediator.Send(new GetGoalByIdQuery(id));
       
        return Ok(goal);

    }

    [HttpPost]
    //[Authorize(PolicyNames.AtLeast18)]
    public async Task<ActionResult<GoalDto>> CreateGoal([FromBody] CreateGoalCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetGoalById), new { id }, null);

    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = UserRoles.User)]

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGoal([FromRoute] int id)
    {
        await mediator.Send(new DeleteGoalCommand(id));
            return NoContent();

       
       

    }
    [HttpPatch("{id}")]
    //[Authorize(Roles = UserRoles.User)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGoal([FromRoute] int id, UpdateGoalCommand command)
    {
        command.Id = id;
       
        await mediator.Send( command);
        
        return NoContent();

      


    }


}
