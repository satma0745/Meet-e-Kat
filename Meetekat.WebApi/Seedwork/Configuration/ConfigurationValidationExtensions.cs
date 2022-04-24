namespace Meetekat.WebApi.Seedwork.Configuration;

using Microsoft.Extensions.Configuration;

public static class ConfigurationValidationExtensions
{
    public static ConfigurationParameter Select(this IConfiguration configuration, string parameterName) =>
        new()
        {
            ParameterName = parameterName,
            StringValue = configuration.GetValue<string>(parameterName)
        };

    public static ConfigurationParameter Required(this ConfigurationParameter configurationParameter) =>
        string.IsNullOrWhiteSpace(configurationParameter.StringValue)
            ? throw InvalidConfigurationException.Required(configurationParameter.ParameterName)
            : configurationParameter;

    public static string AsString(this ConfigurationParameter configurationParameter) =>
        configurationParameter.StringValue;

    public static int AsInt(this ConfigurationParameter configurationParameter) =>
        int.TryParse(configurationParameter.StringValue, out var intValue)
            ? intValue
            : throw InvalidConfigurationException.NotInteger(configurationParameter.ParameterName);
}
