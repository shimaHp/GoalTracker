
using GoalTracker.Domain.Entities;
using GoalTracker.Domain.Repository;
using GoalTracker.Infrastructure.Authorization;
using GoalTracker.Infrastructure.Authorization.Requirement;
using GoalTracker.Infrastructure.Persistence;
using GoalTracker.Infrastructure.Repositories;
using GoalTracker.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GoalTracker.Infrastructure.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        var connectionstring = configuration.GetConnectionString("GoalTrackerDb");

        services.AddDbContext<GoalTrackerDbContext>(options =>
       options.UseSqlServer(connectionstring)
       .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<GoalTrackersUserClaimPrincipalFactory>()
            .AddEntityFrameworkStores<GoalTrackerDbContext>();
        


        services.AddScoped<IGoalTrackerSeeder, GoalTrackerSeeder>();
        services.AddScoped<IGoalsRepository, GoalRepository>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.AtLeast18,
                 builder => builder.AddRequirements(new MinimumAgeRequirment(18)));


        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();






    }
}
