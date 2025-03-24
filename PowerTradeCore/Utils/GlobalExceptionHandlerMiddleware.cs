using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PowerTradeCore;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError($"An error occurred: {exception.Message}");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync(new
        {
            message = "An unexpected error occurred. Please try again later.",
            details = exception?.Message ?? "No details available."
        }.ToString()!);
    }
}
