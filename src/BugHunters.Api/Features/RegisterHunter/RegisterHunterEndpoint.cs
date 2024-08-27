using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values.Hunter;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Features.RegisterHunter;

public class RegisterHunterEndpoint(BugHunterContext context)
    : ApiEndpoint.WithRequest<RegisterHunterEndpoint.RegisterRequest>
{
    [HttpPost("register-hunter")]
    public override async Task<IResult> HandleAsync([FromBody] RegisterRequest request) =>
        await Id<Hunter>.New()
            .ToResult()
            .Map(id => RequestToHunter(request, id))
            .Map(hunter => context.Hunters.Add(hunter).Entity)
            .Tee(async () => await context.TrySaveChangesAsync())
            .Match(
                hunter => Results.Ok(HunterToResponse(hunter)),
                ToProblemDetails
            );

    private static Result<Hunter> RequestToHunter(RegisterRequest request, Id<Hunter> hunterId) =>
        ToObject(
            hunterId.ToResult(),
            DisplayName.FromString(request.Name),
            ViaId.FromString(request.ViaId),
            (id, name, viaId) => new Hunter(id, name, viaId)
        );

    private static RegisterResponse HunterToResponse(Hunter hunter) =>
        new(hunter.Id.Value.ToString(), hunter.DisplayName.Value, hunter.ViaId.Value);

    public record RegisterRequest(string Name, string ViaId);

    public record RegisterResponse(string HunterId, string HunterName, string ViaId);
}