using System.Text.Json.Serialization;

namespace WeatherService.Infrastructure.ExternalApi;

public sealed record WeatherApiResponse(
    [property: JsonPropertyName("temperatureC")] decimal TemperatureC,
    [property: JsonPropertyName("measuredAtUtc")] DateTime MeasuredAtUtc);
