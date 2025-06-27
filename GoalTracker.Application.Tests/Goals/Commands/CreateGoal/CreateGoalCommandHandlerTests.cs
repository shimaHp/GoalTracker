using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using GoalTracker.Application.Goals.Commands.CreateGoal;
using GoalTracker.Domain.Repository;
using GoalTracker.Application.Users;
using GoalTracker.Application.WorkItems.Dtos;
using GoalTracker.Domain.Entities;

using FluentAssertions;
using Shouldly;

namespace GoalTracker.Application.Tests.Goals.Commands.CreateGoal;

public class CreateGoalCommandHandlerTests
{
    private readonly Mock<IGoalsRepository> _goalsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateGoalCommandHandler>> _loggerMock;
    private readonly CreateGoalCommandHandler _handler;

    public CreateGoalCommandHandlerTests()
    {
        _goalsRepositoryMock = new Mock<IGoalsRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateGoalCommandHandler>>();

        _handler = new CreateGoalCommandHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _goalsRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ForValidCommand_ReturnsNewGoalId()
    {
        // Arrange
        var command = new CreateGoalCommand
        {
            Title = "Test Goal",
            Description = "Some description",
            TargetDate = DateTime.UtcNow.AddDays(5),
            CreatedDate = DateTime.UtcNow,
            WorkItems = new List<CreateWorkItemDto>()

        };
        var user = new CurrentUser(
         Id: Guid.NewGuid().ToString(),
        UserName: "test@example.com",
        Email: "test@example.com",
        Roles: new List<string> { "Owner" }, 
        DateOfBirth:null
        );
    

        // Set up the Goal that will be created
        var goal = new Goal
        {
            Id = 1,
            Title = command.Title,
            Description = command.Description,
            TargetDate = command.TargetDate,
            CreatedDate = command.CreatedDate,
            UserId = user.Id
        };
        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(user);

        _mapperMock.Setup(m => m.Map<Goal>(command)).Returns(goal);

        // Simulate repository returning a new goal ID
        _goalsRepositoryMock.Setup(r => r.CreateAsync(goal)).ReturnsAsync(goal.Id);
        //act
        var handler = new CreateGoalCommandHandler(
    _loggerMock.Object,
    _mapperMock.Object,
    _goalsRepositoryMock.Object,
    _userContextMock.Object
);

        var result = await handler.Handle(command, CancellationToken.None);
        // Assert
        result.ShouldBe(goal.Id);
        _goalsRepositoryMock.Verify(r => r.CreateAsync(goal), Times.Once);
     

    }
}













