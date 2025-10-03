using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ShoppingApp.Middlewares.Loggers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ Unhandled exception occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "An unexpected error occurred. Please try again later.",
                    details = ex.Message // ⚠️ optional, remove in production
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
