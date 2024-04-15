using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Entities;

public record Hunter
{
    public Hunter(Id<Hunter> id, Name name, ViaId viaId)
        => (Id, Name, ViaId) = (id, name, viaId);

    public Id<Hunter> Id { get; init; }
    public Name Name { get; init; }
    public ViaId ViaId { get; init; }

    private Hunter()
    {
    } // EFC
}