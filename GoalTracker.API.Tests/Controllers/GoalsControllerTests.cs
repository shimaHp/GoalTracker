using Xunit;
using GoalTracker.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using GoalTracker.Application.Goals.Queries.GetGoalById;
using Microsoft.AspNetCore.Mvc;
using GoalTracker.Application.Goals.Dtos;
using FluentAssertions;

namespace GoalTracker.API.Controllers.Tests
{
    public class GoalsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GoalsController _controller;

        public GoalsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new GoalsController(_mediatorMock.Object);
        }


        [Fact]
        public async Task GetGoalByIdTest_ReturnsGoal_WhenExists()
        {
            // Arrange
            var goalId = 1;
            var expectedGoal = new GoalDto
            {
                Id = goalId,
                Title = "Test Goal",
                Description = "Test Description"
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetGoalByIdQuery>(q => q.Id == goalId), default))
                .ReturnsAsync(expectedGoal);

            // Act
            var result = await _controller.GetGoalById(goalId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedGoal);
        }






        [Fact()]
        public void GetAllTest()
        {

        }

        [Fact()]
        public void GetGoalByIdTest()
        {

        }

        [Fact()]
        public void CreateGoalTest()
        {

        }

        [Fact()]
        public void DeleteGoalTest()
        {

        }

        [Fact()]
        public void UpdateGoalTest()
        {

        }
    }
}