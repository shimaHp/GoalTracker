using System.Reflection.Metadata.Ecma335;
using GoalTracker.Application.Goals;
using GoalTracker.Application.Goals.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GoalTracker.API.Controllers;


[ApiController]
[Route("api/goals")]
public class GoalsController(IGoalsService goalService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var goals= await goalService.GetAllGoals();
        return Ok(goals);
            
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById([FromRoute] int id)
    {
        var goal=await goalService.GetGoalById(id);
        if (goal == null)
            return NotFound();
        return Ok(goal);
            
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody]CreateGoalDto createGoalDto)
    {
        int id = await goalService.CreateGoal(createGoalDto);
        return CreatedAtAction(nameof(GetGoalById), new { id }, null);

    }


}
