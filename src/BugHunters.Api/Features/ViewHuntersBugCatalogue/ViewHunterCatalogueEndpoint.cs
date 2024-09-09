using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Entities.Common;
using BugHunters.Api.Entities.HunterEntity;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Features.ViewHuntersBugCatalogue;

public class ViewHunterCatalogueEndpoint(BugHunterContext context)
    : ApiEndpoint.WithRequest<ViewHunterCatalogueEndpoint.ViewCatalogueRequest>
{
    [HttpGet("hunter-catalogue")]
    public override async Task<IResult> HandleAsync([FromBody] ViewCatalogueRequest request) =>
        await request.HunterId
            .ToResult()
            .Bind(Id<Hunter>.FromString)
            .Where(id => context.HunterExists(id, id))
            .Map(id => LoadCatches(id, context))
            .Map(list => new ViewCatalogueResponse(list))
            .Match(
                Results.Ok,
                ToProblemDetails);

    private static Task<List<BugDto>> LoadCatches(Id<Hunter> id, BugHunterContext ctx) =>
        ctx.BugCatches
            .Where(b => b.HunterId == id)
            .OrderBy(b => b.TimeCaught) // probably won't work because DateTime. Should use string? Should test...
            .Select(b => new BugDto(b.Bug.Name, b.Bug.Description, b.Bug.Image))
            .ToListAsync();

    public record ViewCatalogueRequest(string HunterId);

    private record ViewCatalogueResponse(List<BugDto> Bugs);

    private record BugDto(string Name, string Description, byte[] Image);
}