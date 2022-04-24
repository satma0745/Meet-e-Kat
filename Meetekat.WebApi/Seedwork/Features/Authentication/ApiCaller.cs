namespace Meetekat.WebApi.Seedwork.Features.Authentication;

using System;

/// <summary>All available info about the API Caller.</summary>
public class ApiCaller
{
    /// <summary>API Caller's User ID.</summary>
    /// <example>13371337-1337-1337-1337-133713371337</example>
    public Guid UserId { get; init; }
}
