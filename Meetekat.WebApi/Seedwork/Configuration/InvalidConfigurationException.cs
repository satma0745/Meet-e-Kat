namespace Meetekat.WebApi.Seedwork.Configuration;

using System;

public class InvalidConfigurationException : Exception
{
    public static InvalidConfigurationException Required(string parameterName) =>
        new($"Configuration parameter \"{parameterName}\" is required.");

    public static InvalidConfigurationException NotInteger(string parameterName) =>
        new($"Configuration parameter \"{parameterName}\" must be an integer.");
    
    private InvalidConfigurationException(string message)
        : base(message)
    {
    }
}
