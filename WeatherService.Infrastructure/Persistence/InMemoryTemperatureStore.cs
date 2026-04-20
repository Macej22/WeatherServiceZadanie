using System.Collections.Concurrent;
using WeatherService.Application.Interfaces;
using WeatherService.Domain;

namespace WeatherService.Infrastructure.Persistence;

public sealed class InMemoryTemperatureStore : ITemperatureStore
{
    private readonly ConcurrentDictionary<City, CityTemperatureEntry> _store = new();

    public void Set(CityTemperatureEntry entry) => _store[entry.City] = entry;

    public CityTemperatureEntry? Get(City city) => _store.TryGetValue(city, out var entry) ? entry : null;
}
