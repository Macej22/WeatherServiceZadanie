namespace WeatherService.Domain;

public sealed record CityTemperatureEntry(
    City City,
    decimal TemperatureC,
    DateTime MeasuredAtUtc,
    DateTime CachedAtUtc);
