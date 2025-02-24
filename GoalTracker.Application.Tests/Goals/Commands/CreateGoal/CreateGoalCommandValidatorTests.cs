using Xunit;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace GoalTracker.Application.Goals.Commands.CreateGoal.Tests
{
    public class CreateGoalCommandValidatorTests
    {
        [Fact()]
        public void  Validator_ForValidCommand_ShouldNotHaveValidationErrors()

        {
            //arrange
            var command = new CreateGoalCommand()
            {
                Title = "SqlQuery",
                Description = "Test",
                TargetDate = DateTime.Now.AddDays(1)
                ,
                CreatedDate = DateTime.Today
            };
            var validator = new CreateGoalCommandValidator();
            //act
            var result= validator.TestValidate(command);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
               
            
        }
    

     [Fact()]
        public void Validator_ForValidCommand_ShouldHaveValidationErrors()

        {
            //arrange
            var command = new CreateGoalCommand()
            {
                Title = "",
                Description = "Test",
                TargetDate = DateTime.Now
                ,
                CreatedDate = DateTime.Today
            };
            var validator = new CreateGoalCommandValidator();
            //act
            var result = validator.TestValidate(command);

            //assert
            result.ShouldHaveValidationErrorFor(c=>c.Title);
            result.ShouldHaveValidationErrorFor(c=>c.TargetDate);
       


        }
    }

}