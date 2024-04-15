using BugHunters.Api.Entities.Values;

namespace BugHunters.Api.Entities;

public record BugCatch(Id<Hunter> CaughtBy, Id<Bug> BugCaught, DateTime TimeCaught);