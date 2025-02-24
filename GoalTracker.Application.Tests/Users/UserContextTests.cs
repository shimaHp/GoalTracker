﻿using Xunit;
using GoalTracker.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GoalTracker.Domain.Constants;
using FluentAssertions;

namespace GoalTracker.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
  
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            //arrange
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var httpContextAccessorMock= new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {

                new (ClaimTypes.NameIdentifier,"1"),
                new (ClaimTypes.Email,"test@test.com"),
                new (ClaimTypes.Role,UserRoles.Admin),
                new (ClaimTypes.Role,UserRoles.Owner),
                new("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd"))

            };

            var user= new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));
            httpContextAccessorMock.Setup(x=>x.HttpContext).Returns(new DefaultHttpContext()
            {
              User=user
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);
            //act
            var currentUser=userContext.GetCurrentUser();

            //asset
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin,UserRoles.Owner);
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
          
//
        }

        [Fact]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
            var userContext= new UserContext(httpContextAccessorMock.Object);

            //act

            Action action= () => userContext.GetCurrentUser();
            //asset
            action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");

        }
    }
    
}