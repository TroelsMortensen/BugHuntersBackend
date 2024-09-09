using BugHunters.Api.Common.Endpoint;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ViewHunterProfile;

public class ViewHunterProfileEndpoint
    : ApiEndpoint.WithRequest<ViewHunterProfileEndpoint.ProfileRequest>
{
    [HttpGet("hunter-profile")]
    public override Task<IResult> HandleAsync([FromBody] ProfileRequest request)
    {
        throw new NotImplementedException();
    }

    public record ProfileRequest(string HunterId);
}

public record StringResponse
{
    public StringResponse(string helloWorld)
    {
    }
}