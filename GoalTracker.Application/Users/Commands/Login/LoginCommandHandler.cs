using GoalTracker.Application.Common.Interfaces;
using GoalTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalTracker.Application.Users.Commands.Login;

public class LoginCommandHandler(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = jwtService.GenerateToken(user);

        // Get user roles if needed
        var roles = await userManager.GetRolesAsync(user);

      

       
        return new LoginResponse(
            Token: token,
            UserId: Guid.Parse(user.Id), 
            UserName: user.UserName,
            Email: user.Email,
            Roles: roles
        );

    }
}
