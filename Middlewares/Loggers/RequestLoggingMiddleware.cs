using Serilog;
using System.Diagnostics;

namespace ShoppingApp.Middlewares.Loggers
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.Now;
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Information("➡️ Request In: {Method} {Path} at {RequestTime}",
                context.Request.Method,
                context.Request.Path,
                requestTime);

            await _next(context); // Continue pipeline

            stopwatch.Stop();
            var responseTime = DateTime.Now;

            Log.Information("⬅️ Request Out: {Method} {Path} at {ResponseTime}, Duration: {Elapsed} ms, Status: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                responseTime,
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode);
        }
    }
}
