﻿using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContract;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ViewHunterCatalogue;

public class ViewHunterCatalogueEndpoint(IQueryHandler<ViewCatalogueQuery, ViewCatalogueAnswer> handler)
    : ApiEndpoint
        .WithRequest<ViewCatalogueRequest>
        .WithResponse<ViewCatalogueAnswer>
{
    [HttpGet("catalogue/{HunterId}")]
    public override async Task<ActionResult<ViewCatalogueAnswer>> HandleAsync([FromRoute] ViewCatalogueRequest request)
    {
        Result<ViewCatalogueAnswer> result = await handler.HandleAsync(new ViewCatalogueQuery(request.HunterId));
        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Payload);
    }
}

public record ViewCatalogueRequest([FromRoute] string HunterId);