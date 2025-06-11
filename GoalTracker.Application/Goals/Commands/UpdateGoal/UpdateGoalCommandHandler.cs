

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.Users;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GoalTracker.Application.Goals.Commands.UpdateGoal;



public class UpdateGoalCommandHandler(
    ILogger<UpdateGoalCommandHandler> logger,
    IMapper mapper,
    IGoalsRepository goalsRepository,
    IGoalAuthorizationService goalAuthorizationService
   , IUserContext userContext  
    ) : IRequestHandler<UpdateGoalCommand, GoalDto>

{

    public async Task<GoalDto> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        
        //goal.UserId = currentUser.Id;

        logger.LogInformation("Updating Goal with Title: {Title}", request.UpdateGoalDto.Title);

        // Get goal with work items to avoid N+1 queries
        var goal = await goalsRepository.GetGoalAsync(request.UpdateGoalDto.Id);

        if (goal is null)
            throw new NotFoundException(nameof(Goal), request.UpdateGoalDto.Id.ToString());

        if (!goalAuthorizationService.Authorize(goal, ResourceOperation.Update))
            throw new ForbidException();

        // Update basic goal properties (excluding Id, CreatedDate, WorkItems)
        mapper.Map(request.UpdateGoalDto, goal);
   
       

        // Handle work items if any operations are specified
        if (HasWorkItemOperations(request.UpdateGoalDto))
        {
            await UpdateWorkItemsAsync(goal, request.UpdateGoalDto, cancellationToken);
        }

        await goalsRepository.UpdateAsynce(goal);

        logger.LogInformation("Successfully updated Goal with id: {GoalId}", goal.Id);

        // Return the updated goal as DTO
        return mapper.Map<GoalDto>(goal);
    }

    private static bool HasWorkItemOperations(UpdateGoalDto updateGoalDto)
    {
        return (updateGoalDto.NewWorkItems?.Count > 0) ||
               (updateGoalDto.UpdatedWorkItems?.Count > 0) ||
               (updateGoalDto.DeletedWorkItemIds?.Count > 0);
    }

    private async Task UpdateWorkItemsAsync(Goal goal, UpdateGoalDto updateGoalDto, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var currentUserId = currentUser.Id;
        var now = DateTime.UtcNow;

        // Handle deletions first
        if (updateGoalDto.DeletedWorkItemIds?.Count > 0)
        {
            await HandleWorkItemDeletions(goal, updateGoalDto.DeletedWorkItemIds);
        }

        // Handle updates to existing items
        if (updateGoalDto.UpdatedWorkItems?.Count > 0)
        {
            await HandleWorkItemUpdates(goal, updateGoalDto.UpdatedWorkItems, currentUserId, now);
        }

        // Handle new items
        if (updateGoalDto.NewWorkItems?.Count > 0)
        {
            await HandleWorkItemCreations(goal, updateGoalDto.NewWorkItems, currentUserId, now);
        }
    }

    private async Task HandleWorkItemDeletions(Goal goal, List<int> deletedWorkItemIds)
    {
        var itemsToDelete = goal.WorkItems
            .Where(w => deletedWorkItemIds.Contains(w.Id))
            .ToList();

        foreach (var itemToDelete in itemsToDelete)
        {
            // Option 1: Hard delete
            goal.WorkItems.Remove(itemToDelete);

            // Option 2: Soft delete (if you have IsDeleted property)
            // itemToDelete.IsDeleted = true;
            // itemToDelete.DeletedDate = DateTime.UtcNow;
            // itemToDelete.DeletedById = currentUserService.GetCurrentUserId();
        }

        if (itemsToDelete.Count > 0)
        {
            logger.LogInformation("Deleted {Count} work items from Goal {GoalId}",
                itemsToDelete.Count, goal.Id);
        }

        await Task.CompletedTask; // Placeholder for any async deletion logic
    }

    private async Task HandleWorkItemUpdates(Goal goal, List<UpdateWorkItemDto> updatedWorkItems,
        string currentUserId, DateTime now)
    {
        foreach (var updateWorkItemDto in updatedWorkItems)
        {
            var existingItem = goal.WorkItems.FirstOrDefault(w => w.Id == updateWorkItemDto.Id);

            if (existingItem == null)
            {
                logger.LogWarning("Work item with id {WorkItemId} not found for update in Goal {GoalId}",
                    updateWorkItemDto.Id, goal.Id);
                continue;
            }

            // Use AutoMapper for updates (excludes Id, CreatedDate, etc.)
            mapper.Map(updateWorkItemDto, existingItem);

            // Set audit fields
            existingItem.LastUpdatedDate = now;
            existingItem.LastUpdatedById = currentUserId;
        }

        await Task.CompletedTask; // Placeholder for any async update logic
    }

    private async Task HandleWorkItemCreations(Goal goal, List<CreateWorkItemDto> newWorkItems,
        string currentUserId, DateTime now)
    {
        foreach (var createWorkItemDto in newWorkItems)
        {
            var newWorkItem = mapper.Map<WorkItem>(createWorkItemDto);

            // Set required fields
            newWorkItem.GoalId = goal.Id;
            newWorkItem.CreatedDate = now;
            newWorkItem.CreatorId = currentUserId;

            goal.WorkItems.Add(newWorkItem);
        }

        if (newWorkItems.Count > 0)
        {
            logger.LogInformation("Added {Count} new work items to Goal {GoalId}",
                newWorkItems.Count, goal.Id);
        }

        await Task.CompletedTask; // Placeholder for any async creation logic
    }
}