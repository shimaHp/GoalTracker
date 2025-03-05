

using System.Linq.Expressions;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoalTracker.Infrastructure.Repositories;

internal class GoalRepository(GoalTrackerDbContext dbContext) : IGoalsRepository
{
    
 

    public async Task<(IEnumerable<Goal>, int)> GetAllMatchingAsync(string? searchPhrase
        , int pageSize
        , int pageNumber
        , string? sortBy
        , SortDirection sortDirection
        )
    {


        // Apply defaults if values are null or zero
        searchPhrase ??= "";
        pageSize = pageSize > 0 ? pageSize : 10;
        pageNumber = pageNumber > 0 ? pageNumber : 1;
        sortBy ??= "CreatedDate";
        //--------------------
        var searchPhraseToLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Goals
            //.Goals.Include(r => r.WorkItems)
            .Where(r => searchPhraseToLower == null ||
            (r.Title.ToLower().Contains(searchPhraseToLower)
            || r.Description.ToLower().Contains(searchPhraseToLower)));

        var totalCount = await baseQuery.CountAsync();
        if (sortBy != null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Goal, object>>>
            {

                { nameof(Goal.Title), r=>r.Title}
          
            };
            var selectedColumn = columnSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Asceding
              ? baseQuery.OrderBy(selectedColumn)
              : baseQuery.OrderByDescending(selectedColumn);
        }


        var goals = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();



        return (goals, totalCount);


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
