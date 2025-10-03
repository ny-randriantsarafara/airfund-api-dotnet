using Domain.Models;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            KeyNotFoundException => new ErrorResponse(
                "Resource not found",
                (int)HttpStatusCode.NotFound,
                exception.Message),
            
            ArgumentNullException => new ErrorResponse(
                "Invalid input provided",
                (int)HttpStatusCode.BadRequest,
                exception.Message),
            
            InvalidOperationException => new ErrorResponse(
                "Invalid operation",
                (int)HttpStatusCode.BadRequest,
                exception.Message),
            
            ArgumentException => new ErrorResponse(
                "Invalid argument provided",
                (int)HttpStatusCode.BadRequest,
                exception.Message),
            
            _ => new ErrorResponse(
                "An internal server error occurred",
                (int)HttpStatusCode.InternalServerError,
                "Please try again later")
        };

        response.StatusCode = errorResponse.StatusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await response.WriteAsync(jsonResponse);
    }
}