using BugHunters.Api.Common.Endpoint;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ViewHunterProfile;

public class ViewHunterProfileEndpoint
: ApiEndpoint.WithoutRequest.WithResponse<StringResponse>
{
    [HttpGet("hunterprofile/{HunterId}")]
    public override async Task<ActionResult<StringResponse>> HandleAsync()
    {
        Console.WriteLine("Request received: " + DateTime.Now);
        return new StringResponse("Hello World!");
    }
}

public record StringResponse
{
    public StringResponse(string helloWorld)
    {
        
    }
}

public record ProfileRequest([FromRoute]string HunterId);

