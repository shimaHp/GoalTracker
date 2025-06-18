using FluentValidation;
using GoalTracker.Application.WorkItems.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.WorkItems.Commands.UpdateWorkItem
{
    public class UpdateWorkItemDtoValidator : AbstractValidator<UpdateWorkItemDto>
    {
        public UpdateWorkItemDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Work item ID must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Work item title is required.")
                .MaximumLength(200).WithMessage("Work item title cannot exceed 200 characters.")
                .MinimumLength(1).WithMessage("Work item title must have at least 1 character.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Work item description cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            // Add validation for other properties if they exist in your UpdateWorkItemDto
            // Example:
            // RuleFor(x => x.DueDate)
            //     .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            //     .WithMessage("Work item due date cannot be in the past.")
            //     .When(x => x.DueDate.HasValue);

            // RuleFor(x => x.Priority)
            //     .IsInEnum().WithMessage("Invalid work item priority.")
            //     .When(x => x.Priority.HasValue);

            // RuleFor(x => x.Status)
            //     .IsInEnum().WithMessage("Invalid work item status.")
            //     .When(x => x.Status.HasValue);

            // RuleFor(x => x.CompletedDate)
            //     .LessThanOrEqualTo(DateTime.UtcNow)
            //     .WithMessage("Work item completion date cannot be in the future.")
            //     .When(x => x.CompletedDate.HasValue);
        }
    }
}
