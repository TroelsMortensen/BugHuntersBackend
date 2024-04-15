using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.StrongId;

namespace BugHunters.Api.Entities;

public record BugCatch(Id<Hunter> HunterId, Id<Bug> BugId, DateTime TimeCaught)
{
    public Hunter Hunter { get; set; }
    public Bug Bug { get; set; }
}