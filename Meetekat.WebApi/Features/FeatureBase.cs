namespace Meetekat.WebApi.Features;

using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class FeatureBase : ControllerBase
{
    /// <summary>
    /// Creates an <see cref="ObjectResult"/> object
    /// that produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    protected ObjectResult Created(object value) =>
        StatusCode(StatusCodes.Status201Created, value);
}
