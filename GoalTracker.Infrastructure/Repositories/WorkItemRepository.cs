using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Persistence;

namespace GoalTracker.Infrastructure.Repositories
{
    internal class WorkItemRepository(GoalTrackerDbContext dbContext) : IWorkItemRepository
    {
        public async Task<int> CreateAsync(WorkItem workItem)
        {
           dbContext.Add(workItem);
           await dbContext.SaveChangesAsync();
            return workItem.Id;

        }

        public async Task DeleteAsync(IEnumerable<WorkItem> entities)
        {
            dbContext.WorkItems.RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        


    }
}
