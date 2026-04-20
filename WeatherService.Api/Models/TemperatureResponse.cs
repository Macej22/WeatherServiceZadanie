namespace WeatherService.Api.Models;

public sealed record TemperatureResponse(decimal TemperatureC, DateTime MeasuredAtUtc);
