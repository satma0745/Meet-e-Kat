namespace Meetekat.WebApi.Seedwork.Validation;

using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NotEmptyAttribute : ValidationAttribute
{
    private static readonly DateTime EmptyDateTime = new();

    public NotEmptyAttribute()
        : base("The {0} field is required.")
    {
    }

    public override bool IsValid(object value) =>
        value switch
        {
            null => false,
            string stringValue when string.IsNullOrEmpty(stringValue) => false,
            Guid guidValue when guidValue == Guid.Empty => false,
            DateTime dateTimeValue when dateTimeValue == EmptyDateTime => false,
            _ => true
        };
}
