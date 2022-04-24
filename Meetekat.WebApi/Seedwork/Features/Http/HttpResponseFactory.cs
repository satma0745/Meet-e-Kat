namespace Meetekat.WebApi.Seedwork.Features.Http;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class HttpResponseFactory
{
    #region 2xx Success
    
    /// <inheritdoc cref="ControllerBase.Ok()"/>
    public static OkResult Ok() => new();
    
    /// <inheritdoc cref="ControllerBase.Ok(object)"/>
    public static OkObjectResult Ok(object value) => new(value);
    
    /// <summary>
    /// Creates an <see cref="ObjectResult"/> object
    /// that produces a <see cref="StatusCodes.Status201Created"/> response.
    /// </summary>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
    public static ObjectResult Created(object value) =>
        StatusCode(StatusCodes.Status201Created, value);

    /// <inheritdoc cref="ControllerBase.NoContent"/>
    public static NoContentResult NoContent() => new();
    
    #endregion

    #region 4xx Client-Side error

    /// <inheritdoc cref="ControllerBase.BadRequest()"/>
    public static BadRequestResult BadRequest() => new();

    /// <inheritdoc cref="ControllerBase.Unauthorized()"/>
    public static UnauthorizedResult Unauthorized() => new();

    /// <summary>
    /// Creates an <see cref="ForbidResult"/> that produces a <see cref="StatusCodes.Status403Forbidden"/> response.
    /// </summary>
    /// <returns>The created <see cref="ForbidResult"/> for the response.</returns>
    public static ForbidResult Forbidden() => new();
    
    /// <inheritdoc cref="ControllerBase.NotFound()"/>
    public static NotFoundResult NotFound() => new();
    
    /// <inheritdoc cref="ControllerBase.Conflict()"/>
    public static ConflictResult Conflict() => new();

    #endregion
    
    private static ObjectResult StatusCode(int statusCode, object value) =>
        new(value) {StatusCode = statusCode};
}
