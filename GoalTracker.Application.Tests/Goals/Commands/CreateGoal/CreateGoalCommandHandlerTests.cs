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
            //
            var commandTest = TestObjectMother.CreateGoalCommand(false);
            var expectedGoal = TestObjectMother.CreateGoal();
            //
            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            var handler = new CreateGoalCommandHandler(loggerMock.Object, mapperMock.Object, goalsRepositoryMock.Object, userContextMock.Object);
       

            await FluentActions
        .Awaiting(() => handler.Handle(commandTest, CancellationToken.None))
        .Should()
        .ThrowAsync<NullReferenceException>();

        }
        [Fact]
      public async void CreateGoalCommandHandler_WhenRepositoryThrowsException_PropagatesException()
        {
            //Arrange
            var mapperMock = new Mock<IMapper>();
            var loggerMock= new Mock<ILogger<CreateGoalCommandHandler>>();
            var userContextMock = new Mock<IUserContext>();
            var goalRepositoryMock= new Mock<IGoalsRepository>();
            //
            var commandTest = TestObjectMother.CreateGoalCommand(false);
            var currentUerTest = TestObjectMother.CreateUser();
            var expectedGoal = TestObjectMother.CreateGoal();
            //
            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            userContextMock.Setup(u=>u.GetCurrentUser()).Returns(currentUerTest);
            goalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>())).ThrowsAsync(new InvalidOperationException("Database connection failed"));

            var handler = new CreateGoalCommandHandler(loggerMock.Object, mapperMock.Object, goalRepositoryMock.Object, userContextMock.Object);


            //
            await FluentActions
                .Awaiting(() => handler.Handle(commandTest, CancellationToken.None))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Database connection failed");
                ;
        }

        [Fact]
         public  async void CreateGoalCommandHandler_WhenCreatingGoal_SetsGoalPropertiesCorrectly()
        {
            var mapperMock =new  Mock<IMapper>();
            var loggerMck = new Mock<ILogger<CreateGoalCommandHandler>>();
            var goalRepositoryMock=new Mock<IGoalsRepository>();
            var userContextMock= new Mock<IUserContext>();

            var commandTest = TestObjectMother.CreateGoalCommand(false);
            var expectedGoal = TestObjectMother.CreateGoal();
            Goal captureGoal = null;
            var currentUserTest = TestObjectMother.CreateUser();

            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUserTest);
            goalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>()))
                .Callback<Goal>(goal => captureGoal = goal)
                .ReturnsAsync(1);
            var handler = new CreateGoalCommandHandler(loggerMck.Object, mapperMock.Object, goalRepositoryMock.Object, userContextMock.Object);

            //Act
            var result= await handler.Handle(commandTest,CancellationToken.None);
            //Assert
            captureGoal.Should().NotBeNull();
            captureGoal.UserId.Should().Be(currentUserTest.Id);
            captureGoal.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            result.Should().Be(1);


        }

        [Fact]
        public void CreateGoalCommandHandler_CreateGoalWithGoalItems_setWorkItemsCorrectly()
            {
        
            var loggerMock=new Mock<ILogger<CreateGoalCommandHandler>>();
            var userContextMock= new Mock<IUserContext>();
            var mapperMock=new Mock<IMapper>();
            var goalRepositoryMock = new Mock<IGoalsRepository>();

            var currentUserTest= TestObjectMother.CreateUser();
            var commandTest = TestObjectMother.CreateGoalCommand(true);
            var expectedGoal = TestObjectMother.CreateGoal();
            var expectedWorkItems = TestObjectMother.CreateWorkItems();
            Goal capturedGoal = null;

            userContextMock.Setup(u=>u.GetCurrentUser()).Returns(currentUserTest);
            mapperMock.Setup(m => m.Map<Goal>(commandTest)).Returns(expectedGoal);
            mapperMock.Setup(m => m.Map<List<WorkItem>>(commandTest.WorkItems)).Returns(expectedWorkItems);
            goalRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Goal>()))
                .Callback<Goal>(goal=>capturedGoal=goal)
                .ReturnsAsync(1);

            var handler= new CreateGoalCommandHandler(loggerMock.Object,mapperMock.Object,goalRepositoryMock.Object,userContextMock.Object);
            //Act

            var result= handler.Handle(commandTest,CancellationToken.None);
            //Assert

            capturedGoal.Should().NotBeNull();
            capturedGoal.WorkItems.Should().HaveCount(2);
            var workItems = capturedGoal.WorkItems.ToList();
            // Check first work item
            workItems[0].CreatorId.Should().Be(currentUserTest.Id);
            workItems[0].AssigneeId.Should().Be("Test-asignee-1");
            workItems[0].Goal.Should().Be(capturedGoal);

            workItems[1].CreatorId.Should().Be(currentUserTest.Id);
            workItems[1].AssigneeId.Should().Be("Test-asignee-2");
            workItems[1].Goal.Should().Be(capturedGoal);
            //well done :) 



        }
    }
}