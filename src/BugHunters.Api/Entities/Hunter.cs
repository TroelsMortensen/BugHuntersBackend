using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Entities;

public record Hunter
{
    public Hunter(Id<Hunter> id, Name name, ViaId viaId)
        => (Id, Name, ViaId) = (id, name, viaId);

    public Id<Hunter> Id { get; init; }
    public Name Name { get; init; }
    public ViaId ViaId { get; init; }

    public IEnumerable<BugCatch> BugCatches { get; set; } = new List<BugCatch>();

    private Hunter()
    {
    } // EFC
}