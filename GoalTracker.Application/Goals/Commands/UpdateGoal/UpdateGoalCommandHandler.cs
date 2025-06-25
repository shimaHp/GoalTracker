

using AutoMapper;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.Users;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Enums;
using GoalTracker.Domain.Exceptions;
using GoalTracker.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        logger.LogInformation("Updating Goal with Title: {Title}", request.UpdateGoalDto.Title);

        await goalsRepository.BeginTransactionAsync();

        try
        {
            var goal = await goalsRepository.GetGoalAsync(request.UpdateGoalDto.Id);

            if (goal is null)
                throw new NotFoundException(nameof(Goal), request.UpdateGoalDto.Id.ToString());

            if (!goalAuthorizationService.Authorize(goal, ResourceOperation.Update))
                throw new ForbidException();

            mapper.Map(request.UpdateGoalDto, goal);

            if (HasWorkItemOperations(request.UpdateGoalDto))
            {
                await UpdateWorkItemsAsync(goal, request.UpdateGoalDto, cancellationToken);
                UpdateGoalStatusBasedOnWorkItems(goal);
            }

            await goalsRepository.UpdateAsync(goal);  // Fixed the typo: UpdateAsync instead of UpdateAsynce
            await goalsRepository.CommitTransactionAsync();

            logger.LogInformation("Successfully updated Goal with id: {GoalId}", goal.Id);

            return mapper.Map<GoalDto>(goal);
        }
        catch
        {
            await goalsRepository.RollbackTransactionAsync();
            throw;
        }
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
            HandleWorkItemDeletions(goal, updateGoalDto.DeletedWorkItemIds);
        }

        // Handle updates to existing items
        if (updateGoalDto.UpdatedWorkItems?.Count > 0)
        {
            HandleWorkItemUpdates(goal, updateGoalDto.UpdatedWorkItems, currentUserId, now);
        }

        // Handle new items
        if (updateGoalDto.NewWorkItems?.Count > 0)
        {
            HandleWorkItemCreations(goal, updateGoalDto.NewWorkItems, currentUserId, now);
        }

        // Remove unnecessary await Task.CompletedTask
    }

    private void HandleWorkItemDeletions(Goal goal, List<int> deletedWorkItemIds)
    {
        var itemsToDelete = goal.WorkItems
            .Where(w => deletedWorkItemIds.Contains(w.Id))
            .ToList();

        // Validate that all requested items exist
        var foundIds = itemsToDelete.Select(w => w.Id).ToList();
        var notFoundIds = deletedWorkItemIds.Except(foundIds).ToList();

        if (notFoundIds.Count > 0)
        {
            logger.LogWarning("Work items with ids {NotFoundIds} not found for deletion in Goal {GoalId}",
                string.Join(", ", notFoundIds), goal.Id);
        }

        foreach (var itemToDelete in itemsToDelete)
        {
            goal.WorkItems.Remove(itemToDelete);
        }

        if (itemsToDelete.Count > 0)
        {
            logger.LogInformation("Deleted {Count} work items from Goal {GoalId}",
                itemsToDelete.Count, goal.Id);
        }
    }

    private void HandleWorkItemUpdates(Goal goal, List<UpdateWorkItemDto> updatedWorkItems,
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

            // Store original values for logging
            var originalTitle = existingItem.Title;

            // Use AutoMapper for updates
            mapper.Map(updateWorkItemDto, existingItem);

            // Set audit fields
            existingItem.LastUpdatedDate = now;
            existingItem.LastUpdatedById = currentUserId;

            logger.LogDebug("Updated work item {WorkItemId}: '{OriginalTitle}' -> '{NewTitle}'",
                existingItem.Id, originalTitle, existingItem.Title);
        }
    }
    private void UpdateGoalStatusBasedOnWorkItems(Goal goal)
    {
        if (goal.WorkItems.Any() &&
            goal.WorkItems.All(w => w.Status == WorkItemStatus.Completed))
        {
            goal.Status = GoalStatus.Completed;
            logger.LogInformation("All work items completed. Auto-marked Goal {GoalId} as Completed.", goal.Id);
        }
    }
    private void HandleWorkItemCreations(Goal goal, List<CreateWorkItemDto> newWorkItems,
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
    }
}