using BugHunters.Api.Entities.Values;

namespace BugHunters.Api.Entities;

public record Bug(Id<Bug> Id, string name, string Description, string LocationDescription, byte[] Image);