namespace BugHunters.Api.Entities.Common;

public record Id<T>
{
    // Todo: split this into record, and functions module.
    
    public Guid Value { get; }

    private Id()
    {
        Value = Guid.NewGuid();
    }

    private Id(Guid val)
        => Value = val;

    public static Id<T> New() => new();

    public static Id<T> FromGuid(Guid val) => new(val);

    public static Result<Id<T>> FromString(string val)
    {
        if (Guid.TryParse(val, out Guid guid))
        {
            return new Id<T>(guid);
        }

        return new ResultError("Hunter.Id", "Invalid Id format.");
    }
}