using BugHunters.Api.Entities.Values;

namespace BugHunters.Api.Entities;

public record Hunter(HunterId Id, string Name, ViaId ViaId);