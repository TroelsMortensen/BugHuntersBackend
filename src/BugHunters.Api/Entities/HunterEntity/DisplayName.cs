using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Entities.HunterEntity;

public record DisplayName
{
    public string Value { get; }

    private DisplayName(string value)
        => Value = value;

    public static Result<DisplayName> FromString(string value) =>
        AssertAll(
                IsBelowMaxLength(value),
                IsAboveMinLength(value),
                IsNotEmpty(value) // not really necessary
            )
            .WithPayloadIfSuccess(() => new DisplayName(value));

    private static Func<Result<None>> IsBelowMaxLength(string value) =>
        () =>
            value.Length <= 20
                ? Success()
                : new ResultError("Hunter.Name", "Name must be less than 20 characters.");

    private static Func<Result<None>> IsAboveMinLength(string value) =>
        () =>
            value.Length >= 2
                ? Success()
                : new ResultError("Hunter.Name", "Name must be more than 2 characters.");

    private static Func<Result<None>> IsNotEmpty(string value) =>
        () =>
            !string.IsNullOrWhiteSpace(value)
                ? Success()
                : new ResultError("Hunter.Name", "Name cannot be empty.");


    private DisplayName()
    {
    } // EFC
}