using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoalTracker.Domain.Constants;
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Enums;
using GoalTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoalTracker.Infrastructure.Seeders
{
    internal class GoalTrackerSeeder(GoalTrackerDbContext dbContext) : IGoalTrackerSeeder
    {
        

      

        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Goals.Any())
                {
                    var goals = GetGoals();
                    dbContext.Goals.AddRange(goals);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
            }

        }
        private IEnumerable<IdentityRole> GetRoles()
        {
    //        Admin = 1,      // Complete system control
    //Owner = 2,      // Full project control
    //Collaborator = 3, // Can create and modify goals
    //Viewer = 4
            List<IdentityRole> roles = [  
            new (UserRoles.Owner)
            {
                NormalizedName= UserRoles.Owner.ToUpper(),
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper(),
            },
            new(UserRoles.Collaborator)
            {
                NormalizedName = UserRoles.Collaborator.ToUpper(),
            },
            new(UserRoles.Viewer)
            {
                NormalizedName = UserRoles.Viewer.ToUpper(),
            },

        ];
            return roles;
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
