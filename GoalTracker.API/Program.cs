using GoalTracker.API.Middlewares;
using GoalTracker.Application.Extensions;
using  GoalTracker.Infrastructure.Extension;
using GoalTracker.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;





var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeLogingMiddleware>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Host.UseSerilog((context, confiurtion) => confiurtion.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder=scope.ServiceProvider.GetRequiredService<IGoalTrackerSeeder>();

await seeder.Seed();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLogingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
