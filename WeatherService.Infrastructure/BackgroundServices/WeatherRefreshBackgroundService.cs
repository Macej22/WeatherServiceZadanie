using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using WeatherService.Application.Interfaces;
using WeatherService.Domain;
using WeatherService.Infrastructure.ExternalApi;

namespace WeatherService.Infrastructure.BackgroundServices;

public sealed class WeatherRefreshBackgroundService(
    IServiceScopeFactory scopeFactory,
    ITemperatureStore store,
    ILogger<WeatherRefreshBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("WeatherRefreshBackgroundService started");

        await RefreshAllCitiesAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRefresh = GetNextRefreshTime(DateTime.UtcNow);
            var delay = nextRefresh - DateTime.UtcNow;

            logger.LogInformation("Next weather refresh scheduled at {NextRefresh} UTC (in {Delay:hh\\:mm\\:ss})", nextRefresh, delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await RefreshAllCitiesAsync(stoppingToken);
        }

        logger.LogInformation("WeatherRefreshBackgroundService stopped");
    }

    private async Task RefreshAllCitiesAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Refreshing temperature data for all cities");

        using var scope = scopeFactory.CreateScope();
        var apiClient = scope.ServiceProvider.GetRequiredService<IWeatherApiClient>();

        foreach (var city in Enum.GetValues<City>())
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await TryRefreshCityAsync(apiClient, city, cancellationToken);
        }
    }

    private async Task TryRefreshCityAsync(
        IWeatherApiClient apiClient,
        City city,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await apiClient.GetTemperatureAsync((int)city, cancellationToken);

            store.Set(new CityTemperatureEntry(city, response.TemperatureC, response.MeasuredAtUtc, DateTime.UtcNow));

            logger.LogInformation(
                "Temperature updated for {City}: {Temp}°C (measured at {MeasuredAt} UTC)",
                city, response.TemperatureC, response.MeasuredAtUtc);
        }
        catch (ApiException ex)
        {
            logger.LogWarning(ex,
                "WeatherAPI returned {StatusCode} for {City}. Keeping last cached value.",
                (int)ex.StatusCode, city);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex,
                "Failed to fetch temperature for {City}. Keeping last cached value.", city);
        }
    }

    private static DateTime GetNextRefreshTime(DateTime utcNow)
    {
        var today = DateOnly.FromDateTime(utcNow);
        var currentTime = TimeOnly.FromDateTime(utcNow);

        if (currentTime < new TimeOnly(9, 0))
        {
            return today.ToDateTime(new TimeOnly(9, 0), DateTimeKind.Utc);
        }

        if (currentTime < new TimeOnly(16, 0))
        {
            return today.ToDateTime(new TimeOnly(16, 0), DateTimeKind.Utc);
        }

        return today.AddDays(1).ToDateTime(new TimeOnly(9, 0), DateTimeKind.Utc);
    }
}
