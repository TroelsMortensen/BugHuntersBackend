using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Entities;

public record Hunter(HunterId Id, string Name, ViaId ViaId);