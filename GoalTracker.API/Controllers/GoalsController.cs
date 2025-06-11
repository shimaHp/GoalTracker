using System.Reflection.Metadata.Ecma335;
using GoalTracker.Application.Common;
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
//[Authorize]
public class GoalsController(IMediator mediator ) : ControllerBase
{
    [HttpGet]
    //[Authorize(PolicyNames.AtLeast18)]
    //[AllowAnonymous]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<GoalDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<GoalDto>>> GetAll([FromQuery] GetAllGoalsQuery query)
    {
        var goals= await mediator.Send(query); 
        return Ok(goals);
            
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GoalDto>> GetGoalById([FromRoute] int id)
    {
        var goal = await mediator.Send(new GetGoalByIdQuery(id));       
        return Ok(goal);

    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GoalDto>> CreateGoal([FromBody] CreateGoalCommand command)
    {
        var createdGoalId = await mediator.Send(command);

        // Fetch the complete created goal
        var createdGoal = await mediator.Send(new GetGoalByIdQuery(createdGoalId));

        return CreatedAtAction(
            nameof(GetGoalById),
            new { id = createdGoalId },
            createdGoal
        );
    }

    [HttpDelete("{id}")]
    
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGoal([FromRoute] int id)
    {
        await mediator.Send(new DeleteGoalCommand(id));
        return NoContent();

    }

    // Single endpoint to update goal with its work items
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GoalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GoalDto>> UpdateGoal([FromRoute] int id, [FromBody] UpdateGoalDto updateGoalDto)
    {
        // Ensure the ID from route matches the DTO
        if (id != updateGoalDto.Id)
        {
            return BadRequest("Route ID must match the goal ID in the request body");
        }

        var command = new UpdateGoalCommand
        {
            UpdateGoalDto = updateGoalDto
        };

        var updatedGoal = await mediator.Send(command);
        return Ok(updatedGoal);
    }
}

