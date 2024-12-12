

using AutoMapper;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoalTracker.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(
    ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
    ) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UnassignUserRoleCommand user role:{@request}:", request);
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName)
              ?? throw new NotFoundException(nameof(roleManager), request.RoleName);

        await userManager.RemoveFromRoleAsync(user!, role.Name!);

    }
}
