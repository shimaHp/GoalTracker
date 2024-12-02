

using GoalTracker.Application.Goals;
using GoalTracker.Domain.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoalTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
  
        services.AddScoped<IGoalsService, GoalsService>();
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

    }
}