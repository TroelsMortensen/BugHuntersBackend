using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Common.HandlerContracts;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values.Hunter;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace BugHunters.Api.Features.ChangeDisplayName;

public class ChangeDisplayNameEndpoint(BugHunterContext context)
    : ApiEndpoint
        .WithRequest<ChangeDisplayNameEndpoint.ChangeDisplayNameRequest>
{
    [HttpPost("update-displayname")]
    public override async Task<IResult> HandleAsync([FromBody] ChangeDisplayNameRequest request) =>
        await Id<Hunter>.FromString(request.HunterId)
            .Bind(id => context.Hunters.SingleOrFailureAsync(h => h.Id == id))
            .Bind(hunter => ChangeDisplayName(hunter, request.NewDisplayName))
            .Map<Hunter, Hunter>(hunter => context.Hunters.Update(hunter).Entity)
            .Tee(context.TrySaveChangesAsync)
            .Match(
                _ => Results.NoContent(),
                ToProblemDetails
            );


    public static Result<Hunter> ChangeDisplayName(Hunter hunter, string newDisplayName) =>
        DisplayName.FromString(newDisplayName)
            .Map(name => hunter with { DisplayName = name });


    public record ChangeDisplayNameRequest(string HunterId, string NewDisplayName);
}