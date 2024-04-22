namespace BugHunters.Api.Entities.Values.Hunter;

public record DisplayName
{
    public string Value { get; }

    private DisplayName(string value)
        => Value = value;

    public static Result<DisplayName> FromString(string value)
        => Result.StartValidation<DisplayName>()
            .AssertThat(() => !string.IsNullOrWhiteSpace(value), new ResultError("Hunter.Name", "Name cannot be empty."))
            .AssertThat(() => value.Length <= 20, new ResultError("Hunter.Name", "Name must be less than 20 characters."))
            .WithPayloadIfSuccess(() => new DisplayName(value));
    
    private DisplayName(){} // EFC
}