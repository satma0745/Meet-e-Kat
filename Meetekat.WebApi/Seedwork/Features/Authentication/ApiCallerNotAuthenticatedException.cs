namespace Meetekat.WebApi.Seedwork.Features.Authentication;

using System;

public class ApiCallerNotAuthenticatedException : Exception
{
    public ApiCallerNotAuthenticatedException()
        : base("API Caller isn't authenticated.")
    {
    }
}
