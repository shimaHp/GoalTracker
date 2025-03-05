using Microsoft.OpenApi.Models;
using GoalTracker.API.Middlewares;
using Serilog;


namespace GoalTracker.API.Extension;

public static class WebApplicationBuilderExtentions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {

            Reference= new OpenApiReference{Type = ReferenceType.SecurityScheme,Id="bearerAuth"}
        },
        []
      }
    });
        });
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", 
        builder => builder
           //.WithOrigins("https://localhost:7005")
           .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

    }
}