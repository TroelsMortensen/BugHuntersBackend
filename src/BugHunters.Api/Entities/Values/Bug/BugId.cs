namespace BugHunters.Api.Entities.Values.Bug;

public class BugId
{
    public Guid Value { get; }

    private BugId(Guid guid)
        => Value = guid;

    public static Result<BugId> FromString(string value)
        => Guid.TryParse(value, out _)
            ? new BugId(Guid.Parse(value))
            : new ResultError("Bug.Id", "Invalid Bug ID format. Must be a valid GUID.");
}