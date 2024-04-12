using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContract;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.Hunters.RegisterHunter;

public class RegisterHunterEndpoint(ICommandHandler<RegisterHunterCommand> handler)
    : ApiEndpoint.WithRequest<RegisterHunterEndpoint.Request>.WithoutResponse
{
    public record Request(string Name, string ViaId);

    [HttpPost("/hunters/register")]
    public override async Task<ActionResult> HandleAsync([FromBody]Request request)
    {
        string id = Guid.NewGuid().ToString();
        RegisterHunterCommand command = new (id, request.Name, request.ViaId);
        Result<None> result = await handler.HandleAsync(command);
        
        return result.IsSuccess 
            ? Ok(id) 
            : BadRequest(result.Errors);
    }
}