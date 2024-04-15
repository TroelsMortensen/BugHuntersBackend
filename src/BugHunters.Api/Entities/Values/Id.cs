namespace BugHunters.Api.Entities.Values;

public record Id<T>
{
    public Guid Value { get; }

    private Id()
    {
        Value = Guid.NewGuid();
    }

    private Id(Guid val)
        => Value = val;

    public static Id<T> New() => new();

    public static Id<T> FromGuid(Guid val) => new(val);

    public static Result<Id<Entities.Hunter>> FromString(string val)
    {
        if (Guid.TryParse(val, out Guid guid))
        {
            return new Id<Entities.Hunter>(guid);
        }

        return new ResultError("Hunter.Id", "Invalid Id format.");
    }
}