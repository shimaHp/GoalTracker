
using GoalTracker.Infrastructure.Persistence;
using GoalTracker.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GoalTracker.Infrastructure.Extension;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionstring = configuration.GetConnectionString("GoalTrackerDb");
        services.AddDbContext<GoalTrackerDbContext>(Options => Options.UseSqlServer(connectionstring));
        services.AddScoped<IGoalTrackerSeeder, GoalTrackerSeeder>();


    }
}
