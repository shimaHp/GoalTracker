

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using GoalTracker.Domain.Entities;
using System.Security.Claims;

namespace GoalTracker.Infrastructure.Authorization;

public class GoalTrackersUserClaimPrincipalFactory(UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options) :
    UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        // return base.CreateAsync(user);
        var id = await GenerateClaimsAsync(user);
      

        if (user.DateOfBirth != null)
        {
            id.AddClaim(new Claim(AppClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
            
        }
        return new ClaimsPrincipal(id);
    }
}