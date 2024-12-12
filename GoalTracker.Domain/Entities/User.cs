
using Microsoft.AspNetCore.Identity;

namespace GoalTracker.Domain.Entities;

public class User:IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }
    public DateTime? CreatedAt { get; set; }
}
