namespace BugHunters.Api.Entities;

public record Hunter(HunterId Id, string Name, StudentNumber Number);

public record HunterId(Guid Value);

public record StudentNumber(string Value);