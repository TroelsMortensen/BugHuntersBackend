using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Entities;

public record Hunter(HunterId Id, Name Name, ViaId ViaId);