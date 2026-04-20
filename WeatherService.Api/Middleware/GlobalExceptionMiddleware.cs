using System.Net;
using System.Text.Json;
using WeatherService.Application.Exceptions;

namespace WeatherService.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (CityNotFoundException ex)
        {
            _logger.LogWarning("City not found: {CityName}", ex.CityName);
            await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DataNotAvailableException ex)
        {
            _logger.LogWarning("Data not yet available for city: {City}", ex.City);
            await WriteErrorAsync(context, HttpStatusCode.ServiceUnavailable, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var body = JsonSerializer.Serialize(new { error = message });
        
        return context.Response.WriteAsync(body);
    }
}
