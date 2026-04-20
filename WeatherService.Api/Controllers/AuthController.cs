using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherService.Api.Models;
using WeatherService.Api.Settings;
using WeatherService.Application.Interfaces;

namespace WeatherService.Api.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(
    IOptions<AuthSettings> authSettings,
    IJwtTokenService jwtTokenService,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetToken([FromBody] TokenRequest request)
    {
        if (request.ApiKey != authSettings.Value.ApiKey)
        {
            logger.LogWarning("Invalid API key attempt");
            return Unauthorized(new { error = "Invalid API key." });
        }

        var token = jwtTokenService.GenerateToken();
        logger.LogInformation("JWT token issued");
        
        return Ok(new TokenResponse(token));
    }
}
