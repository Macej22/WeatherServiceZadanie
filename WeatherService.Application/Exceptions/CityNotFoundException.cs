namespace WeatherService.Application.Exceptions;

public sealed class CityNotFoundException : Exception
{
    public string CityName { get; }

    public CityNotFoundException(string cityName)
        : base($"City '{cityName}' is not supported. Supported values: bratislava, praha, budapest, vieden.")
    {
        CityName = cityName;
    }
}
