namespace BugHunters.Api.Entities.Values.Hunter;

public record Name
{
    public string Value { get; }

    private Name(string value)
        => Value = value;

    public static Result<Name> FromString(string value)
        => Result.StartValidation<Name>()
            .AssertThat(() => !string.IsNullOrWhiteSpace(value), new ResultError("Hunter.Name", "Name cannot be empty."))
            .AssertThat(() => value.Length <= 20, new ResultError("Hunter.Name", "Name must be less than 20 characters."))
            .WithPayloadIfSuccess(() => new Name(value));
}