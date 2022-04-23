namespace Meetekat.WebApi.Features;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Tags("HealthCheck")]
public class HealthCheckFeature : ControllerBase
{
    [HttpGet("/api/health-checks")]
    [SwaggerOperation("Check if application is up and running.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The application is working normally.")]
    public IActionResult Check() => Ok();
}
