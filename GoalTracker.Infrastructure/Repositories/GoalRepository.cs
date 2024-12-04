

using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoalTracker.Infrastructure.Repositories;

internal class GoalRepository(GoalTrackerDbContext dbContext) : IGoalsRepository
{
    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        var goals = await dbContext.Goals.Include(c => c.WorkItems).ToListAsync();
        return goals;
    }

    public async Task<Goal?> GetGoalAsync(int GId)
    {
        var goal = await dbContext.Goals
            .Include(c=>c.WorkItems)
            .FirstOrDefaultAsync(g => g.Id == GId)
            ;
        return goal;
    }


    
    public async Task<int> CreateAsync(Goal goal)
    {
        dbContext.Goals.Add(goal);
        await dbContext.SaveChangesAsync();
        return goal.Id; 
    }

    public async Task DeleteAsync(Goal goal)
    {
        dbContext.Remove(goal);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsynce(Goal goal)
    {
        dbContext.Goals.Update(goal);
        await dbContext.SaveChangesAsync();
        return goal.Id;
    }

    public Task SaveChanges()
    
       =>dbContext.SaveChangesAsync();
     
}
