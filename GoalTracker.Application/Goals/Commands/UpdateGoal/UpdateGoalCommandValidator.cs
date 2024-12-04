

using FluentValidation;
using GoalTracker.Application.Goals.Commands.editGoal;

namespace GoalTracker.Application.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandValidator:AbstractValidator<UpdateGoalCommand>
{
    public UpdateGoalCommandValidator()
    {
        RuleFor(x => x.Title)
             .NotEmpty().WithMessage("Title is required.")
             .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
    }

}
