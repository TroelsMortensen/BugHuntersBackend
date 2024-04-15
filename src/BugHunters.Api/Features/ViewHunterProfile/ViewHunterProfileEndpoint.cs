using BugHunters.Api.Common.Endpoint;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ViewHunterProfile;

public class ViewHunterProfileEndpoint
: ApiEndpoint.WithoutRequest.WithResponse<StringResponse>
{
    [HttpGet("test")]
    public override async Task<ActionResult<StringResponse>> HandleAsync()
    {
        Console.WriteLine("Request received: " + DateTime.Now);
        return new StringResponse("Hello World!");
    }
}

public record StringResponse(string Value);
