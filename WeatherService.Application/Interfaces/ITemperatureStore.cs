using WeatherService.Domain;

namespace WeatherService.Application.Interfaces;

public interface ITemperatureStore
{
    void Set(CityTemperatureEntry entry);
    CityTemperatureEntry? Get(City city);
}
