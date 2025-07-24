using Xunit;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoalTracker.Application.Users;
using GoalTracker.Domain.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using System.Security.Cryptography.Xml;
using GoalTracker.Domain.Entities;
using FluentAssertions;
using GoalTracker.Application.Tests;

namespace GoalTracker.Application.Goals.Commands.CreateGoal.Tests
{
    public class CreateGoalCommandHandlerTests
    {  
     
       
        [Fact()]
        public async void CreateGoalCommandHandler_createGoalWithutWorkItem_ReturnGoalId()
        {
           //arrange
           var loggerMock= new Mock<ILogger<CreateGoalCommandHandler>>();

            var mapperMock = new Mock<IMapper>();
            var command = TestObjectMother.CreateGoalCommandWithoutWorkItems();
            var goal = TestObjectMother.CreateTestGoal();
            mapperMock.Setup(m=>m.Map<Goal>(command)).Returns(goal);


            var goalRepositoryMock = new Mock<IGoalsRepository>();
            goalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>())).ReturnsAsync(1);

            var userContextMock = new Mock<IUserContext>();
            var currentUser = TestObjectMother.CreateTestUser();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);
            var commandHandler= new CreateGoalCommandHandler(loggerMock.Object,mapperMock.Object,goalRepositoryMock.Object, userContextMock.Object);    

            //act

            var result= await commandHandler.Handle(command,CancellationToken.None);

            //assert

            result.Should().Be(1);
            goalRepositoryMock.Verify(r=>r.CreateAsync(goal),Times.Once());


        }

        [Fact()]
        public void HandleTest()
        {

        }
    }
}