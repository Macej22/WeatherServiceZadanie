using WeatherService.Domain;

namespace WeatherService.Application.Interfaces;

public sealed record TemperatureResult(decimal TemperatureC, DateTime MeasuredAtUtc);

public interface ITemperatureService
{
    Task<TemperatureResult> GetTemperatureAsync(City city, CancellationToken cancellationToken = default);
}
