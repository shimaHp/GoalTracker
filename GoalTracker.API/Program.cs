using GoalTracker.API.Extension;
using GoalTracker.API.Middlewares;
using GoalTracker.Application.Extensions;
using GoalTracker.Domain.Entities;
using GoalTracker.Infrastructure.Extension;
using GoalTracker.Infrastructure.Seeders;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

using Serilog;
using Newtonsoft.Json.Converters;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);


   

    builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       });

    builder.Services.AddSwaggerGen(c =>
    {
        c.UseInlineDefinitionsForEnums();
    });
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






    // Ensure this order
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowClient");
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