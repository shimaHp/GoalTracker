using GoalTracker.Domain.Entities;


namespace GoalTracker.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
