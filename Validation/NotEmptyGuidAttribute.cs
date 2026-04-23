using System.ComponentModel.DataAnnotations;

namespace LibraryBookBorrowingSystem.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class NotEmptyGuidAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is Guid guid && guid != Guid.Empty;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} is required.";
    }
}
