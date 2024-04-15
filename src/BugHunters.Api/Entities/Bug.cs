using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.StrongId;

namespace BugHunters.Api.Entities;

public record Bug(Id<Bug> Id, string Name, string Description, string LocationDescription, byte[] Image);