using Refit;

namespace WeatherService.Infrastructure.ExternalApi;

public interface IWeatherApiClient
{
    [Get("/{cityId}")]
    Task<WeatherApiResponse> GetTemperatureAsync(int cityId, CancellationToken cancellationToken = default);
}
