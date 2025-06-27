using Xunit;
using GoalTracker.Infrastructure.Authorization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using GoalTracker.Domain.Entities;

using GoalTracker.Application.Users;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain;

namespace GoalTracker.Infrastructure.Authorization.Services.Tests
{
    public class GoalAuthorizationServiceTests
    {
        private readonly Mock<ILogger<GoalAuthorizationService>> _loggerMock;
        private readonly Mock<IUserContext> _userContextMock;
            private readonly GoalAuthorizationService _service;


        public GoalAuthorizationServiceTests()
        {
            _loggerMock = new Mock<ILogger<GoalAuthorizationService>>();
            _userContextMock = new Mock<IUserContext>();
            _service = new GoalAuthorizationService(_loggerMock.Object, _userContextMock.Object);
        }


        [Fact]
        public void Authorize_AdminUser_ShouldReturnTrue()
        {
            // Arrange
            var user = new CurrentUser(
                Id: "123",
                UserName: "admin",
                Email: "admin@example.com",
                Roles: new List<string> { UserRoles.Admin },
                DateOfBirth: null
            );
            _userContextMock.Setup(x => x.GetCurrentUser()).Returns(user);

            var goal = new Goal { Title = "Test Goal", UserId = "456" };

            // Act
            var result = _service.Authorize(goal, ResourceOperation.Update);

            // Assert
            Xunit.Assert.True(result);
        }
        [Fact]
        public void Authorize_ViewerCannotCreateOrUpdate_ShouldReturnFalse()
        {
            // Arrange
            var user = new CurrentUser(
                Id: "123",
                UserName: "viewer",
                Email: "viewer@example.com",
                Roles: new List<string> { UserRoles.Viewer },
                DateOfBirth: null
            );
            _userContextMock.Setup(x => x.GetCurrentUser()).Returns(user);

            var goal = new Goal { Title = "Test Goal", UserId = "123" };

            // Act
            var createResult = _service.Authorize(goal, ResourceOperation.Create);
            var updateResult = _service.Authorize(goal, ResourceOperation.Update);

            // Assert
            Xunit.Assert.False(createResult);
            Xunit.Assert.False(updateResult);
        }
    }
}