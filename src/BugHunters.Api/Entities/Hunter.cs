using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Entities;

public record Hunter
{
    public Hunter(HunterId id, Name name, ViaId viaId)
        => (Id, Name, ViaId) = (id, name, viaId);

    public HunterId Id { get; init; }
    public Name Name { get; init; }
    public ViaId ViaId { get; init; }

    private Hunter()
    {
    } // EFC
}