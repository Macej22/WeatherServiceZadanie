using Microsoft.Extensions.Logging;
using WeatherService.Application.Exceptions;
using WeatherService.Application.Interfaces;
using WeatherService.Domain;

namespace WeatherService.Application.Services;

public sealed class TemperatureService(
    ITemperatureStore store,
    ILogger<TemperatureService> logger) : ITemperatureService
{
    public async Task<TemperatureResult> GetTemperatureAsync(City city, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(city))
        {
            throw new CityNotFoundException(city.ToString());
        }

        var entry = store.Get(city);
        if (entry is null)
        {
            logger.LogWarning("No temperature data cached yet for {City}", city);
            throw new DataNotAvailableException(city);
        }

        return new TemperatureResult(Math.Round(entry.TemperatureC, 2), entry.MeasuredAtUtc);
    }
}
