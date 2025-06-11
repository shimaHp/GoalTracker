using FluentValidation;

namespace GoalTracker.Application.WorkItems.Commands.CreateWorkItem
{


    public class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
    {
        public CreateWorkItemCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("DueDate cannot be in the past.")
                .When(x => x.DueDate.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status must be a valid WorkItemStatus value.");

        }
    }
}