namespace BugHunters.Api.Entities;

public record Bug(Id<Bug> Id, string Name, string Description, string LocationDescription, byte[] Image)
{
    public IEnumerable<BugCatch> BugCatches { get; set; } = new List<BugCatch>();
}