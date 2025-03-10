using GoalTracker.API.Extension;
using GoalTracker.API.Middlewares;
using GoalTracker.Application.Extensions;
using GoalTracker.Domain.Entities;
using GoalTracker.Infrastructure.Extension;
using GoalTracker.Infrastructure.Seeders;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IGoalTrackerSeeder>();
    await seeder.Seed();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowClient");

    // Remove this line:
    // app.MapGroup("api/identity").WithTags("Identity").MapIdentityApi<User>();

    // Ensure this order
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}