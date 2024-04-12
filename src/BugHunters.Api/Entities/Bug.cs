using BugHunters.Api.Entities.Values.Bug;

namespace BugHunters.Api.Entities;

public record Bug(BugId Id, string name, string Description, string LocationDescription, byte[] Image);