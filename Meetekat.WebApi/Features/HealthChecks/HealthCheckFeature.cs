namespace Meetekat.WebApi.Features.HealthChecks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

public class HealthCheckFeature : FeatureBase
{
    [Tags("HealthCheck")]
    [HttpGet("/api/health-checks")]
    [SwaggerOperation("Check if application is up and running.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The application is working normally.")]
    public IActionResult Check() => Ok();
}
