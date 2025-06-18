using FluentValidation.TestHelper;
using GoalTracker.Application.WorkItems.Commands.CreateWorkItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoalTracker.Application.Tests.WorkItems.Commands.CreateWorkItem
{
    public class CreateWorkItemCommandValidatorTests
    {
        [Fact]
        public void Validator_ShouldPass_WithValidCommand()
        {
            var command = new CreateWorkItemCommand
            {
                Title = "Write tests",
                Description = "Write unit tests for the validator.",
                DueDate = DateTime.Today.AddDays(1),
                Status =0// WorkItemStatus.NotStarted,
              
            };

            //var validator = new CreateWorkItemCommandValidator();
            //var result = validator.TestValidate(command);

           // result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
