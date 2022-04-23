namespace Meetekat.WebApi.Features;

using System;
using System.Collections.Generic;
using System.Net.Mime;
using Meetekat.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Tags("Meetups")]
[Produces(MediaTypeNames.Application.Json)]
public class GetMeetupsFeature : ControllerBase
{
    [HttpGet("/api/meetups")]
    [SwaggerOperation("Get all meetups.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Meetups retrieved successfully.", typeof(IEnumerable<Meetup>))]
    public IActionResult GetMeetups() =>
        throw new NotImplementedException();
}
