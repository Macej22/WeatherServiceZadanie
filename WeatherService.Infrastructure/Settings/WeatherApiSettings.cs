namespace WeatherService.Infrastructure.Settings;

public sealed class WeatherApiSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public bool UseFake { get; init; }
}
