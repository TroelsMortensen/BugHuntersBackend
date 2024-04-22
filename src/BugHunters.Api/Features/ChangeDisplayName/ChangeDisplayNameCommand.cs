namespace BugHunters.Api.Features.ChangeDisplayName;

public record ChangeDisplayNameCommand(string HunterId, string NewDisplayName);