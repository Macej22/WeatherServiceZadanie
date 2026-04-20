using Microsoft.Extensions.DependencyInjection;
using Refit;
using WeatherService.Application.Interfaces;
using WeatherService.Infrastructure.BackgroundServices;
using WeatherService.Infrastructure.ExternalApi;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Settings;

namespace WeatherService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, WeatherApiSettings settings)
    {
        services.AddSingleton<ITemperatureStore, InMemoryTemperatureStore>();

        if (settings.UseFake)
        {
            services.AddSingleton<IWeatherApiClient, FakeWeatherApiClient>();
        }
        else
        {
            services.AddRefitClient<IWeatherApiClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl));
        }

        services.AddHostedService<WeatherRefreshBackgroundService>();

        return services;
    }
}
