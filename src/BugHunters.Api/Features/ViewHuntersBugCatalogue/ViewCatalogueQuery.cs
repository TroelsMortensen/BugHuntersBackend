namespace BugHunters.Api.Features.ViewHuntersBugCatalogue;

public record ViewCatalogueQuery(string HunterId);

public record ViewCatalogueAnswer(List<BugDto> Bugs);

public record BugDto(string Name, string Description, byte[] Image);