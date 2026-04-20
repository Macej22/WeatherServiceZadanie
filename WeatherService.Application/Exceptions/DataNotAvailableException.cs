using WeatherService.Domain;

namespace WeatherService.Application.Exceptions;

public sealed class DataNotAvailableException : Exception
{
    public City City { get; }

    public DataNotAvailableException(City city)
        : base($"Temperature data for '{city}' is not available yet. Please try again later.")
    {
        City = city;
    }
}
