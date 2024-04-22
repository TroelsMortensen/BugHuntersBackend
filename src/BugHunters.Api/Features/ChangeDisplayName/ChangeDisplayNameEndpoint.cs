using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContracts;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ChangeDisplayName;

public class ChangeDisplayNameEndpoint(ICommandHandler<ChangeDisplayNameCommand> handler)
    : ApiEndpoint
        .WithRequest<ChangeDisplayNameRequest>
        .WithoutResponse
{
    [HttpPost("update-displayname")]
    public override async Task<ActionResult> HandleAsync([FromBody] ChangeDisplayNameRequest request)
    {
        ChangeDisplayNameCommand cmd = new(request.HunterId, request.NewDisplayName);
        Result<None> result = await handler.HandleAsync(cmd);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Errors);
    }
}

public record ChangeDisplayNameRequest(string HunterId, string NewDisplayName);