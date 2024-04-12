namespace BugHunters.Api.Entities.Values.Bug;

public class BugId
{
    public static Result<Bug> FromString(string value)
        => Guid.TryParse(value, out _)
            ? new HunterId(Guid.Parse(value))
            : new ResultError("Hunter.Id", "Invalid HunterId format. Must be a valid GUID.");
}