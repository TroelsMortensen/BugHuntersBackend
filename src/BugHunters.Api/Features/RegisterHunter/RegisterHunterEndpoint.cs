using BugHunters.Api.Features.Common.Endpoint;
using BugHunters.Api.Features.Common.HandlerContract;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.RegisterHunter;

public class RegisterHunterEndpoint(ICommandHandler<RegisterHunterCommand> handler)
    : ApiEndpoint.WithRequest<RegisterHunterEndpoint.Request>.WithoutResponse
{
    public record Request(string Name, string StudentNumber);

    [HttpPost("/hunters/register")]
    public override async Task<ActionResult> HandleAsync([FromBody]Request request)
    {
        Guid id = Guid.NewGuid();
        RegisterHunterCommand command = new (id, request.Name, request.StudentNumber);
        Result<None> result = await handler.HandleAsync(command);
        
        return result.IsSuccess 
            ? Ok(id) 
            : BadRequest(result.Errors);
    }
}