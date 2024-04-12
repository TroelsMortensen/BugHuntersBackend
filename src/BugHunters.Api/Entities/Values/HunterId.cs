namespace BugHunters.Api.Entities.Values;

public record HunterId(Guid Value)
{
    public static Result<HunterId> FromString(string value)
        => Guid.TryParse(value, out _)
            ? new HunterId(Guid.Parse(value))
            : new ResultError("Hunter.Id", "Invalid HunterId format. Must be a valid GUID.");
}