using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Entities.Values.Hunter;

public record DisplayName
{
    public string Value { get; }

    private DisplayName(string value)
        => Value = value;

    public static Result<DisplayName> FromString(string value) =>
        StartValidation()
            .AssertThat(() => !IsEmpty(value), new ResultError("Hunter.Name", "Name cannot be empty."))
            .AssertThat(() => IsBelowMaxLength(value), new ResultError("Hunter.Name", "Name must be less than 20 characters."))
            .AssertThat(() => IsAboveMinLength(value), new ResultError("Hunter.Name", "Name must be more than 2 characters."))
            .WithPayloadIfSuccess(() => new DisplayName(value));

    private static bool IsAboveMinLength(string value) =>
        value.Length >= 2;

    private static bool IsEmpty(string value) =>
        !string.IsNullOrWhiteSpace(value);

    private static bool IsBelowMaxLength(string value) =>
        value.Length <= 20;

    private DisplayName()
    {
    } // EFC
}