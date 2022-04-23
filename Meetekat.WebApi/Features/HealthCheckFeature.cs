namespace Meetekat.WebApi.Features;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Tags("HealthCheck")]
public class HealthCheckFeature : ControllerBase
{
    /// <summary>Check if application is up and running.</summary>
    /// <response code="200">The application is working normally.</response>
    [HttpGet("/api/health-checks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Check() => Ok();
}
