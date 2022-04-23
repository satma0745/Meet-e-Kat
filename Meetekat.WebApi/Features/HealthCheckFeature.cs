namespace Meetekat.WebApi.Features;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HealthCheckFeature : ControllerBase
{
    [HttpGet("/api/health-checks")]
    public IActionResult Check() => Ok();
}
