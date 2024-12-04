using System.Reflection.Metadata.Ecma335;
using GoalTracker.Application.Goals;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.DeleteGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.Goals.Queries.GetAllGoals;
using GoalTracker.Application.Goals.Queries.GetGoalById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoalTracker.API.Controllers;


[ApiController]
[Route("api/goals")]
public class GoalsController(IMediator mediator ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var goals= await mediator.Send(new GetAllGoalsQuery()); 
        return Ok(goals);
            
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById([FromRoute] int id)
    {
        var goal = await mediator.Send(new GetGoalByIdQuery(id));
        if (goal == null)
            return NotFound();
        return Ok(goal);

    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetGoalById), new { id }, null);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteGoalCommand(id));
        if (isDeleted )

            return NoContent();

            return NotFound();
       

    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateGoal([FromRoute] int id, UpdateGoalCommand command)
    {
        command.Id = id;
        var isUpdated = await mediator.Send( command);
        if (isUpdated)

            return NoContent();

        return NotFound();


    }


}
