using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContract;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugEndpoint(ICommandHandler<CatchBugCommand> handler)
    : ApiEndpoint
        .WithRequest<CatchBugRequest>
        .WithoutResponse
{
    [HttpPost("catch-bug")]
    public override async Task<ActionResult> HandleAsync(CatchBugRequest request)
    {
        Result<None> result = await handler.HandleAsync(new CatchBugCommand(request.BugId, request.HunterId));
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Errors);
    }
}

public record CatchBugRequest(string BugId, string HunterId);

