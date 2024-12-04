

using FluentValidation;
using GoalTracker.Application.Goals.Dtos;

namespace GoalTracker.Application.Goals.Commands.CreateGoal;

public class CreateGoalCommandValidator : AbstractValidator<CreateGoalCommand>
{

    public CreateGoalCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.CreatedDate)
            .NotEmpty().WithMessage("CreatedDate is required.");

        RuleFor(x => x.TargetDate)
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("TargetDate cannot be in the past.")
            .When(x => x.TargetDate.HasValue);

        RuleFor(x => x.Status)
            .InclusiveBetween(1, 5).WithMessage("Status must be between 1 and 5.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5.")
            .When(x => x.Priority.HasValue);

    }
}

