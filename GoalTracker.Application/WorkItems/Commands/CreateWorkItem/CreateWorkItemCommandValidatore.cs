using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace GoalTracker.Application.WorkItems.Commands.CreateWorkItem
{
    public class CreateWorkItemCommandValidatore:AbstractValidator<CreateWorkItemCommand>
    {
        public CreateWorkItemCommandValidatore()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            //RuleFor(x => x.DueDate)
            //    .GreaterThan(x => x.CreatedDate).When(x => x.DueDate.HasValue)
            //    .WithMessage("Due date must be after creation date");

            RuleFor(x => x.Status)
    .IsInEnum()
    .WithMessage("Status must be a valid WorkItemStatus value: Pending, InProgress, Completed, or Cancelled.");
        }
    }
    }

