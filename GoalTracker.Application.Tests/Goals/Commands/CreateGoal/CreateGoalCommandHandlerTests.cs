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
using System.Reflection.Metadata;
using Assert = Xunit.Assert;

namespace GoalTracker.Application.Goals.Commands.CreateGoal.Tests
{
    public class CreateGoalCommandHandlerTests
    {


        [Fact()]
        public async void CreateGoalCommandHandler_createGoalWithutWorkItem_ReturnGoalId()
        {
            //A
            var loggerMock = new Mock<ILogger<CreateGoalCommandHandler>>();
            var mapperMock = new Mock<IMapper>();
            var userContextMock = new Mock<IUserContext>();
            var goalsRepositoryMock = new Mock<IGoalsRepository>();

            var commandTest = TestObjectMother.CreateGoalCommand(withWorkItems: false);
            var currentUserTest = TestObjectMother.CreateUser();
            var expectedGoal = TestObjectMother.CreateGoal();

            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUserTest);
            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            goalsRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>())).ReturnsAsync(1);

            var handler = new CreateGoalCommandHandler(loggerMock.Object, mapperMock.Object, goalsRepositoryMock.Object, userContextMock.Object);
            //A
            var result = await handler.Handle(commandTest, CancellationToken.None);

            //Assert
            result.Should().Be(1);
            goalsRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Goal>()), Times.Once);



        }

        [Fact()]
        public async void CreateGoalCommandHandler_createGoalWithWorkItems_ReturnGoalId()
        {

            var mapperMock = new Mock<IMapper>();
            var goalsRepositoryMock = new Mock<IGoalsRepository>();
            var userContextMock = new Mock<IUserContext>();
            var loggerMock = new Mock<ILogger<CreateGoalCommandHandler>>();
            //
            var currentUserTest = TestObjectMother.CreateUser();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUserTest);

            var expectedGoal = TestObjectMother.CreateGoal();
            var expectedWorkItems = TestObjectMother.CreateWorkItems();
            var commandTest = TestObjectMother.CreateGoalCommand(true);
            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            mapperMock.Setup(m => m.Map<List<WorkItem>>(commandTest.WorkItems)).Returns(expectedWorkItems);


            goalsRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>())).ReturnsAsync(1);
            var handler = new CreateGoalCommandHandler(loggerMock.Object, mapperMock.Object, goalsRepositoryMock.Object, userContextMock.Object);
            // Arange
            var result = await handler.Handle(commandTest, CancellationToken.None);

            //assert
            result.Should().Be(1);
            goalsRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Goal>(g =>
                g.UserId == currentUserTest.Id &&
                g.WorkItems.Count == expectedWorkItems.Count &&
                g.WorkItems.All(w => w.CreatorId == currentUserTest.Id)
                )), Times.Once);
            //I enjoyed
        }

        [Fact()]
        public async void CreateGoalCommandHandler_WhenUserContextReturnsNull_ThrowsException()
        {
            //Arange
            var loggerMock = new Mock<ILogger<CreateGoalCommandHandler>>();
            var mapperMock = new Mock<IMapper>();
            var userContextMock = new Mock<IUserContext>();
             var goalsRepositoryMock = new Mock<IGoalsRepository>();

            var commandTest = TestObjectMother.CreateGoalCommand(false);
            var expectedGoal = TestObjectMother.CreateGoal();

            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            var handler = new CreateGoalCommandHandler(loggerMock.Object, mapperMock.Object, goalsRepositoryMock.Object, userContextMock.Object);
          //Act +Assert
           var act= async()=> await handler.Handle(commandTest,CancellationToken.None);
            await act.Should().ThrowAsync<NullReferenceException>();





        }
    }
}