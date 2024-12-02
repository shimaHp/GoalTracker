

using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoalTracker.Infrastructure.Repositories;

internal class GoalRepository(GoalTrackerDbContext dbContext) : IGoalsRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        var goals = await dbContext.Goals.ToListAsync();
        return goals;
    }

    public async Task<Goal?> GetGoalAsync(int GId)
    {
        var goal = await dbContext.Goals
            
            .FirstOrDefaultAsync(g => g.Id == GId)
            ;
        return goal;
    }
}
