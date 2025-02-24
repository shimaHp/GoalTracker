

using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;

namespace GoalTracker.Domain.Repository;

public interface IGoalsRepository
{
    Task<int> CreateAsync(Goal goal);
    Task<IEnumerable<Goal>> GetAllAsync();

    Task<Goal?> GetGoalAsync(int GId);
    Task DeleteAsync(Goal goal);

    Task<int> UpdateAsynce(Goal goal);
    //
    Task<(IEnumerable<Goal>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);



    Task SaveChanges();
}
