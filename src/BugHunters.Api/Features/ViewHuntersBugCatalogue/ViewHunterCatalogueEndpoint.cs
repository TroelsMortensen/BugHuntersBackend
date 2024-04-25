using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContracts;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ViewHuntersBugCatalogue;

public class ViewHunterCatalogueEndpoint(IQueryHandler<ViewCatalogueQuery, ViewCatalogueAnswer> handler)
    : ApiEndpoint
        .WithRequest<ViewCatalogueRequest>
        .WithResponse<ViewCatalogueAnswer>
{
    [HttpGet("hunter/{HunterId}/catalogue")]
    public override async Task<ActionResult<ViewCatalogueAnswer>> HandleAsync([FromRoute] ViewCatalogueRequest request)
    {
        Console.WriteLine("Request received: " + request.HunterId);
        Result<ViewCatalogueAnswer> result = await handler.HandleAsync(new ViewCatalogueQuery(request.HunterId));
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Payload);
    }
}

public record ViewCatalogueRequest([FromRoute] string HunterId);