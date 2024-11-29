using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Enums;
using GoalTracker.Infrastructure.Persistence;

namespace GoalTracker.Infrastructure.Seeders
{
    internal class GoalTrackerSeeder : IGoalTrackerSeeder
    {
        private readonly GoalTrackerDbContext _dbContext;

        public GoalTrackerSeeder(GoalTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.Goals.Any())
                {
                    var goals = GetGoals();
                    _dbContext.Goals.AddRange(goals);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Goal> GetGoals()
        {
            var goals = new List<Goal>
    {
        new Goal
        {
            Title = "Learn Blazor Fundamentals",
            Description = "Master Blazor web development",
            CreatedDate = DateTime.UtcNow,
            Status = GoalStatus.InProgress,
            Priority = Priority.High,
            WorkItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Title = "Complete Blazor Tutorial",
                    Description = "Follow official Microsoft Blazor documentation",
                    CreatedDate = DateTime.UtcNow,
                    Status = WorkItemStatus.Pending,
                    Goal = null // Temporarily null; will be auto-linked by EF when added to the Goal's `WorkItems`
                },
                new WorkItem
                {
                    Title = "Build Simple Component",
                    Description = "Create first Blazor component from scratch",
                    CreatedDate = DateTime.UtcNow,
                    Status = WorkItemStatus.Pending,
                    Goal = null
                }
            }
        },
        new Goal
        {
            Title = "Complete Portfolio Project",
            Description = "Develop a full-stack task management application",
            CreatedDate = DateTime.UtcNow,
            Status = GoalStatus.InProgress,
            Priority = Priority.Critical,
            WorkItems = new List<WorkItem>
            {
                new WorkItem
                {
                    Title = "Set Up Database Context",
                    Description = "Configure Entity Framework Core",
                    CreatedDate = DateTime.UtcNow,
                    Status = WorkItemStatus.Completed,
                    Goal = null
                },
                new WorkItem
                {
                    Title = "Design User Interface",
                    Description = "Create Blazor components for goal tracking",
                    CreatedDate = DateTime.UtcNow,
                    Status = WorkItemStatus.InProgress,
                    Goal = null
                }
            }
        }
    };

            // EF Core automatically sets the `Goal` property for each WorkItem
            // when saving because `WorkItems` are part of the `Goal` entity.

            return goals;
        }

    }
}
