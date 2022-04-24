namespace Meetekat.WebApi.Seedwork.Features;

using System;
using System.Net.Mime;
using JetBrains.Annotations;
using Meetekat.WebApi.Seedwork.Features.Authentication;
using Meetekat.WebApi.Seedwork.Features.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class FeatureBase
{
    #region API Caller

    /// <summary>Contains current Api Caller's info.</summary>
    /// <returns>A <see cref="ApiCaller"/> instance containing User's info.</returns>
    protected ApiCaller Caller => lazyApiCallerInfoAccessor.Value.ApiCaller;

    private readonly Lazy<ApiCallerInfoAccessor> lazyApiCallerInfoAccessor;

    #endregion

    #region API Controller

    /// <inheritdoc cref="ControllerBase.ControllerContext"/>
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    [ControllerContext]
    public ControllerContext ControllerContext
    {
        get => controllerContext ??= new ControllerContext();
        set => controllerContext = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    private ControllerContext controllerContext;
    
    #endregion

    protected FeatureBase() =>
        lazyApiCallerInfoAccessor = new Lazy<ApiCallerInfoAccessor>(() =>
        {
            var user = ControllerContext.HttpContext.User;
            return new ApiCallerInfoAccessor(user.Claims);
        });

    #region HTTP Responses

    /// <inheritdoc cref="HttpResponseFactory.Ok()"/>
    protected static OkResult Ok() =>
        HttpResponseFactory.Ok();
    
    /// <inheritdoc cref="HttpResponseFactory.Ok(object)"/>
    protected static OkObjectResult Ok(object value) =>
        HttpResponseFactory.Ok(value);
    
    /// <inheritdoc cref="HttpResponseFactory.Created"/>
    protected static ObjectResult Created(object value) =>
        HttpResponseFactory.Created(value);

    /// <inheritdoc cref="HttpResponseFactory.NoContent"/>
    protected static NoContentResult NoContent() =>
        HttpResponseFactory.NoContent();

    /// <inheritdoc cref="HttpResponseFactory.BadRequest"/>
    protected static BadRequestResult BadRequest() =>
        HttpResponseFactory.BadRequest();

    /// <inheritdoc cref="HttpResponseFactory.Unauthorized"/>
    protected static UnauthorizedResult Unauthorized() =>
        HttpResponseFactory.Unauthorized();

    /// <inheritdoc cref="HttpResponseFactory.Forbidden"/>
    protected static ForbidResult Forbidden() =>
        HttpResponseFactory.Forbidden();

    /// <inheritdoc cref="HttpResponseFactory.NotFound"/>
    protected static NotFoundResult NotFound() =>
        HttpResponseFactory.NotFound();

    /// <inheritdoc cref="HttpResponseFactory.Conflict"/>
    protected static ConflictResult Conflict() =>
        HttpResponseFactory.Conflict();

    #endregion
}
