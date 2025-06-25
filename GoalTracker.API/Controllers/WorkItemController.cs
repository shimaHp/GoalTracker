
using GoalTracker.Application.Goals.Queries.GetAllGoals;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;
using GoalTracker.Application.WorkItems.Commands.DeleteWorkItems;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Application.WorkItems.Queries.GetWorkItemByIdForGoal;
using GoalTracker.Application.WorkItems.Queries.GetWorkItemsForGoal;
using GoalTracker.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTracker.API.Controllers;

[ApiController]
[Route("api/goals/{goalId}/workitems")]
public class WorkItemController(IMediator mediator) : ControllerBase
{

  

    //[HttpPost]
    //[Authorize(Policy = ResourcePolicies.WorkItem.Create)]
    //public async Task<IActionResult> CreateWorkItem([FromRoute] int goalId, CreateWorkItemCommand command)
    //{
    //  //  command.GoalId = goalId;
    //  var workItemId =  await mediator.Send(command);
    //    return   CreatedAtAction(nameof(GetByIdForGoal), new { goalId,workItemId },null);
    //}
    // GET: api/goals/{goalId}/workitems

    [HttpGet]
    [Authorize(Policy = ResourcePolicies.WorkItem.Read)]
    public async Task<ActionResult<IEnumerable<WorkItemDto>>> GetAllForGoal([FromRoute] int goalId)
    {
        var workItems = await mediator.Send(new GetWorkItemsForGoalQuery(goalId));
        return Ok(workItems);
    }

    // GET: api/goals/{goalId}/workitems/{workItemId}
    [HttpGet("{workItemId}")]
    [Authorize(Policy = ResourcePolicies.WorkItem.Read)]
    public async Task<ActionResult<WorkItemDto>> GetByIdForGoal([FromRoute] int goalId, [FromRoute] int workItemId)
    {
        var workItem = await mediator.Send(new GetWorkItemByIdForGoalQuery(goalId, workItemId));
        return Ok(workItem);
    }

    [HttpDelete]
    [Authorize(Policy = ResourcePolicies.WorkItem.Delete)]
    public async Task<IActionResult> DeleteAllForGoal([FromRoute] int goalId)
    {
      await mediator.Send(new DeleteWorkItemsForGoalCommand(goalId));
        return NoContent();
    }

    //[HttpPatch("{workItemId}")]
    //[Authorize(Policy = ResourcePolicies.WorkItem.Update)]
    //public async Task<IActionResult> UpdateWorkItemForGoal([FromRoute] int workItemId)
    //{
    //    await mediator.Send(new UpdateWorkItemCommand(workItemId));
    //    return NoContent();
    //}



}
