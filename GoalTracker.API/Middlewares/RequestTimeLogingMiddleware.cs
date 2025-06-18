using System.Diagnostics;

namespace GoalTracker.API.Middlewares
{
    public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();
            await next.Invoke(context);
            stopWatch.Stop();

            // Log if request takes more than 4 seconds
            if (stopWatch.ElapsedMilliseconds > 4000)
            {
                logger.LogWarning("Slow request [{Verb}] at {Path} took {Time} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopWatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogInformation("Request [{Verb}] at {Path} completed in {Time} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopWatch.ElapsedMilliseconds);
            }
        }
    }
}