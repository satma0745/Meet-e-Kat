namespace Meetekat.WebApi.Seedwork.Configuration;

using System;

public class InvalidConfigurationException : Exception
{
    public static InvalidConfigurationException Required(string parameterName) =>
        new($"Configuration parameter \"{parameterName}\" is required.");

    public static InvalidConfigurationException NotInt(string parameterName) =>
        new($"Configuration parameter \"{parameterName}\" must be an integer.");

    public static InvalidConfigurationException NotBool(string parameterName) =>
        new($"Configuration parameter \"{parameterName}\" must be a boolean.");
    
    private InvalidConfigurationException(string message)
        : base(message)
    {
    }
}
