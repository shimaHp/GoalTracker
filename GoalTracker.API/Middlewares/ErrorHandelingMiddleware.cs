

using FluentValidation;
using GoalTracker.Domain.Exceptions;
using System.Text.Json;

namespace GoalTracker.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException validationEx)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var errors = validationEx.Errors.Select(e => new {
                Property = e.PropertyName,
                Message = e.ErrorMessage
            });

            var response = new { Errors = errors };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            logger.LogWarning("Validation failed: {ValidationErrors}",
                string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage)));
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var response = new { Error = notFound.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            logger.LogWarning("Resource not found: {Message}", notFound.Message);
        }
        catch (ForbidException)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var response = new { Error = "Access forbidden" };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            logger.LogWarning("Access forbidden for user {UserId}",
                context.User?.Identity?.Name ?? "Unknown");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new { Error = "An internal server error occurred" };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}