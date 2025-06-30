

using System.Linq.Expressions;
using GoalTracker.Application.Goals.Dtos;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GoalTracker.Infrastructure.Repositories;

internal class GoalRepository(GoalTrackerDbContext dbContext) : IGoalsRepository
{
    private IDbContextTransaction? _currentTransaction;


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
        sortBy ??= "CreatedAt";
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

                { nameof(Goal.Title), r=>r.Title},
                { "CreatedAt", r => r.CreatedDate } // Add this line
          
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
    .Include(g => g.WorkItems)
        .ThenInclude(w => w.Creator)       // Include Creator user
    .Include(g => g.WorkItems)
        .ThenInclude(w => w.Assignee)      // Include Assignee user
    .Include(g => g.WorkItems)
        .ThenInclude(w => w.LastUpdatedBy) // Include last updated by user
    .Include(g => g.User)                  // Include Goal's User property if needed
    .FirstOrDefaultAsync(g => g.Id == GId);

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

    public async Task<int> UpdateAsync(Goal goal)
    {
        dbContext.Goals.Update(goal);
        await dbContext.SaveChangesAsync();
        return goal.Id;
    }

    public Task SaveChanges()
    
       =>dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("Transaction already started");

        _currentTransaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction to commit");

        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No transaction to rollback");

        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }
}
