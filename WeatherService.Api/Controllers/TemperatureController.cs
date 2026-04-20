using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Api.Models;
using WeatherService.Application.Interfaces;
using WeatherService.Domain;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("api/temperature")]
[Authorize]
public sealed class TemperatureController(
    ITemperatureService temperatureService) : ControllerBase
{
    /// <summary>
    /// Returns the current temperature for the given city.
    /// </summary>
    /// <param name="city">Supported values: bratislava, praha, budapest, vieden</param>
    [HttpGet("{city}")]
    [ProducesResponseType(typeof(TemperatureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetTemperature(string city, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<City>(city, ignoreCase: true, out var parsedCity))
        {
            return NotFound(new { error = $"City '{city}' is not supported. Supported values: bratislava, praha, budapest, vieden." });
        }

        var result = await temperatureService.GetTemperatureAsync(parsedCity, cancellationToken);
        
        return Ok(new TemperatureResponse(result.TemperatureC, result.MeasuredAtUtc));
    }
}
