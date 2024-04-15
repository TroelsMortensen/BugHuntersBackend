using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContracts;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.RegisterHunter;

public class RegisterHunterEndpoint(ICommandHandler<RegisterHunterCommand> handler)
    : ApiEndpoint.WithRequest<RegisterHunterEndpoint.RegisterRequest>.WithResponse<RegisterHunterEndpoint.RegisterResponse>
{
    [HttpPost("register-hunter")]
    public override async Task<ActionResult<RegisterResponse>> HandleAsync([FromBody] RegisterRequest request)
    {
        string id = Guid.NewGuid().ToString();
        RegisterHunterCommand command = new(id, request.Name, request.ViaId);
        Result<None> result = await handler.HandleAsync(command);

        return result.IsSuccess
            ? Ok(new RegisterResponse(id))
            : BadRequest(result.Errors);
    }

    public record RegisterRequest(string Name, string ViaId);

    public record RegisterResponse(string Id);
}