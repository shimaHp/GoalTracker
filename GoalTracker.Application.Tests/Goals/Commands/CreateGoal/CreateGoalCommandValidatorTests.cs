using Xunit;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using GoalTracker.Application.Tests;
using Assert = Xunit.Assert;
using Shouldly;
using GoalTracker.Domain.Enums;

namespace GoalTracker.Application.Goals.Commands.CreateGoal.Tests
{
    public class CreateGoalCommandValidatorTests
    {
        private readonly CreateGoalCommandValidator _validator;
        public CreateGoalCommandValidatorTests()
        {
            _validator = new CreateGoalCommandValidator();
        }

        [Fact]
        public void CreateGoalCommandValidator_ValidCommand_shouldPass()
        {
            //Arrange
            var command = TestObjectMother.CreateGoalCommand(false);

            //Act
            var result=_validator.Validate(command);
            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);

        }

        [Theory]
        [InlineData("", "Title is required.")]
        [InlineData("This is a very long title that definitely exceeds the maximum length limit of 100 characters for sure", "Title cannot exceed 100 characters.")]
        public void CreateGoalCommandValidator_InvalidTitle_ShouldFail(string invalidTitle,string expectedError)
        {
            //Arrange
            var commandTest = TestObjectMother.CreateGoalCommand(false);
            commandTest.Title = invalidTitle;
            //Act
            var result=_validator.Validate(commandTest);
            //Assert
            result.IsValid.ShouldBeFalse();
        
            Assert.Contains(result.Errors,error=>error.ErrorMessage==expectedError);
        }

        [Fact]
        public void Validate_InvalidDescription_ShouldFail()
        {
            // Arrange
            var command = TestObjectMother.CreateGoalCommand();
            command.Description = new string('a', 501); // Exceeds 500 chars

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Description cannot exceed 500 characters.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("A short description.")]
        public void Validate_ValidDescriptions_ShouldPass(string? description)
        {
            var command = TestObjectMother.CreateGoalCommand();
            command.Description = description;

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }
        [Fact]
        public void Validate_TargetDateInPast_ShouldFail()
        {
            // Arrange
            var command = TestObjectMother.CreateGoalCommand();
            command.TargetDate = DateTime.UtcNow.AddDays(-1); // Past date

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == "TargetDate cannot be in the past.");
        }
        [Fact]
        public void Validate_InvalidStatuse_ShouldFail()
        {
            // Arrange
            var command = TestObjectMother.CreateGoalCommand();
            command.TargetDate = DateTime.UtcNow.AddDays(-1); // Past date

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == "TargetDate cannot be in the past.");
        }
        [Theory]
        [InlineData(GoalStatus.NotStarted)]
        [InlineData(GoalStatus.InProgress)]
        [InlineData(GoalStatus.Completed)]
        [InlineData(GoalStatus.OnHold)]
        [InlineData(GoalStatus.Cancelled)]
        public void Validate_ValidStatusValues_ShouldPass(GoalStatus status)
        {
            var command = TestObjectMother.CreateGoalCommand();
            command.Status = status;

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }
        [Fact]
        public void Validate_InvalidStatusValue_ShouldFail()
        {
            var command = TestObjectMother.CreateGoalCommand();
            command.Status = (GoalStatus)999; // Invalid enum value

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Status must be a valid GoalStatus value.");
        }
        [Theory]
        [InlineData(Priority.Low)]
        [InlineData(Priority.Medium)]
        [InlineData(Priority.High)]
        [InlineData(Priority.Critical)]
        public void Validate_ValidPriority_ShouldPass(Priority priority)
        {
            var command = TestObjectMother.CreateGoalCommand();
            command.Priority = priority;

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }
        [Fact]
        public void Validate_InvalidPriority_ShouldFail()
        {
            var command = TestObjectMother.CreateGoalCommand();
            command.Priority = (Priority)(-1); // Invalid enum value

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Priority must be a valid Priority value.");
        }

    }


}