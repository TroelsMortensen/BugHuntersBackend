using BugHunters.Api.Entities.Common;

namespace BugHunters.Api.Entities.HunterEntity;

public record Hunter
{
    public Hunter(Id<Hunter> id, DisplayName displayName, ViaId viaId)
        => (Id, DisplayName, ViaId) = (id, displayName, viaId);

    public Id<Hunter> Id { get; init; }
    public DisplayName DisplayName { get; init; }
    public ViaId ViaId { get; init; }

    public IEnumerable<BugCatch> BugCatches { get; set; } = new List<BugCatch>();

    private Hunter()
    {
    } // EFC
}