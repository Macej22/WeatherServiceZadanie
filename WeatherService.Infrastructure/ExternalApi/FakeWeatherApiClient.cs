namespace WeatherService.Infrastructure.ExternalApi;

public sealed class FakeWeatherApiClient : IWeatherApiClient
{
    private static readonly Random _rng = Random.Shared;

    public async Task<WeatherApiResponse> GetTemperatureAsync(int cityId, CancellationToken cancellationToken = default)
    {
        var temp = Math.Round((decimal)(_rng.NextDouble() * 35 - 5), 2);
        var response = new WeatherApiResponse(temp, DateTime.UtcNow);
        
        return response;
    }
}
