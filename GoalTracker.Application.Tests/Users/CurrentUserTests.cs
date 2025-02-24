
using FluentAssertions;
using GoalTracker.Domain.Constants;
using Xunit;
namespace GoalTracker.Application.Users.Tests;

public class CurrentUserTests
{
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.Owner)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        //arrange
        var currentUser= new CurrentUser("1", "test@test.com", [UserRoles.Admin,UserRoles.Owner],null);
        //act
        var isInRole= currentUser.IsInRole(roleName);
        //assert


        isInRole.Should().BeTrue();


    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.Owner], null);
        //act
        var isInRole = currentUser.IsInRole(UserRoles.Viewer);
        //assert

        isInRole.Should().BeFalse();
            }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.Owner], null);
        //act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());
        //assert

        isInRole.Should().BeFalse();
    }

}