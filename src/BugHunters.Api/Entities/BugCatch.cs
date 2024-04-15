using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.StrongId;

namespace BugHunters.Api.Entities;

public record BugCatch(Id<Hunter> CaughtBy, Id<Bug> BugCaught, DateTime TimeCaught);