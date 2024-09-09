using BugHunters.Api.Entities.Common;
using BugHunters.Api.Entities.HunterEntity;

namespace BugHunters.Api.Entities;

public record BugCatch(Id<Hunter> HunterId, Id<Bug> BugId, DateTime TimeCaught)
{
    public Hunter Hunter { get; set; } // TODO should I try out Option<Hunter> here? Or just =null! that shit
    public Bug Bug { get; set; }
}