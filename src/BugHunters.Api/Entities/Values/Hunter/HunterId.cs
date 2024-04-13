namespace BugHunters.Api.Entities.Values.Hunter;

public record HunterId
{
    public Guid Value { get; }

    private HunterId(Guid guid)
        => Value = guid;

    public static Result<HunterId> FromString(string value)
        => Guid.TryParse(value, out _)
            ? new HunterId(Guid.Parse(value))
            : new ResultError("Hunter.Id", "Invalid HunterId format. Must be a valid GUID.");

    public static Result<HunterId> FromGuid(Guid value)
        => FromString(value.ToString());
    
    private HunterId(){} // EFC
}