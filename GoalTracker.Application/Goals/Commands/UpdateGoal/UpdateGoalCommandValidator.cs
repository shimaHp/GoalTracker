

using FluentValidation;
using GoalTracker.Application.Goals.Commands.editGoal;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;
using GoalTracker.Application.WorkItems.Commands.UpdateWorkItem;
using GoalTracker.Application.WorkItems.Dtos;

namespace GoalTracker.Application.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandValidator : AbstractValidator<UpdateGoalCommand>
{
    public UpdateGoalCommandValidator()
    {
        // Goal basic properties validation
        RuleFor(x => x.UpdateGoalDto.Id)
            .GreaterThan(0).WithMessage("Goal ID must be greater than 0.");

        RuleFor(x => x.UpdateGoalDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.")
            .MinimumLength(1).WithMessage("Title must have at least 1 character.");

        RuleFor(x => x.UpdateGoalDto.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.UpdateGoalDto.Description));

        RuleFor(x => x.UpdateGoalDto.TargetDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Target date cannot be in the past.")
            .When(x => x.UpdateGoalDto.TargetDate.HasValue);

        RuleFor(x => x.UpdateGoalDto.Status)
            .IsInEnum().WithMessage("Invalid goal status.");

        RuleFor(x => x.UpdateGoalDto.Priority)
            .IsInEnum().WithMessage("Invalid priority level.");

        // Work item operations validation
        RuleFor(x => x.UpdateGoalDto)
            .Must(HaveReasonableWorkItemOperations)
            .WithMessage("Too many work item operations in a single request (maximum 100 total).");

        // New work items validation
        RuleForEach(x => x.UpdateGoalDto.NewWorkItems)
            .SetValidator(new CreateWorkItemDtoValidator())
            .When(x => x.UpdateGoalDto.NewWorkItems != null);

        // Updated work items validation
        RuleForEach(x => x.UpdateGoalDto.UpdatedWorkItems)
            .SetValidator(new UpdateWorkItemDtoValidator())
            .When(x => x.UpdateGoalDto.UpdatedWorkItems != null);

        // Deleted work item IDs validation
        RuleForEach(x => x.UpdateGoalDto.DeletedWorkItemIds)
            .GreaterThan(0).WithMessage("Work item ID must be greater than 0.")
            .When(x => x.UpdateGoalDto.DeletedWorkItemIds != null);

        // Ensure no duplicate work item IDs in updates
        RuleFor(x => x.UpdateGoalDto.UpdatedWorkItems)
            .Must(HaveUniqueWorkItemIds)
            .WithMessage("Duplicate work item IDs found in update list.")
            .When(x => x.UpdateGoalDto.UpdatedWorkItems != null && x.UpdateGoalDto.UpdatedWorkItems.Count > 1);

        // Ensure no duplicate work item IDs in deletions
        RuleFor(x => x.UpdateGoalDto.DeletedWorkItemIds)
            .Must(HaveUniqueIds)
            .WithMessage("Duplicate work item IDs found in deletion list.")
            .When(x => x.UpdateGoalDto.DeletedWorkItemIds != null && x.UpdateGoalDto.DeletedWorkItemIds.Count > 1);
    }

    private static bool HaveReasonableWorkItemOperations(UpdateGoalDto dto)
    {
        var totalOperations = (dto.NewWorkItems?.Count ?? 0) +
                            (dto.UpdatedWorkItems?.Count ?? 0) +
                            (dto.DeletedWorkItemIds?.Count ?? 0);

        return totalOperations <= 100;
    }

    private static bool HaveUniqueWorkItemIds(List<UpdateWorkItemDto>? workItems)
    {
        if (workItems == null) return true;

        var ids = workItems.Select(w => w.Id).ToList();
        return ids.Count == ids.Distinct().Count();
    }

    private static bool HaveUniqueIds(List<int>? ids)
    {
        if (ids == null) return true;

        return ids.Count == ids.Distinct().Count();
    }
}