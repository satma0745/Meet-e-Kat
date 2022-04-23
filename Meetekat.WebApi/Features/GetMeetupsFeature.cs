namespace Meetekat.WebApi.Features;

using System.Collections.Generic;
using System.Net.Mime;
using Meetekat.WebApi.Entities;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Tags("Meetups")]
[Produces(MediaTypeNames.Application.Json)]
public class GetMeetupsFeature : ControllerBase
{
    private readonly ApplicationContext context;

    public GetMeetupsFeature(ApplicationContext context) =>
        this.context = context;

    [HttpGet("/api/meetups")]
    [SwaggerOperation("Get all meetups.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<Meetup>))]
    public IActionResult GetMeetups() =>
        Ok(context.Meetups);
}
