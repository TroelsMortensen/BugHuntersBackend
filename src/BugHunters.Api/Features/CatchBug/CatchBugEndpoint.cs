using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugEndpoint(BugHunterContext context)
    : ApiEndpoint.WithRequest<CatchBugRequest>
{
    [HttpPost("catch-bug")]
    public override async Task<IResult> HandleAsync(CatchBugRequest request)
    {
        ToTuple(request)
            .Where(hunterIdAndBugId => BugNotAlreadyCaught(hunterIdAndBugId, context))
            .Map(tuple => LoadEntitiesAsync(tuple, context))
    }

    private static async Task<Result<(Id<Hunter> HunterId, Id<Bug> BugId)>> BugNotAlreadyCaught(
        (Id<Hunter> HunterId, Id<Bug> BugId) ids,
        BugHunterContext ctx
    ) =>
        await ctx.BugCatches.SingleOrFailureAsync(cb => cb.BugId == ids.BugId && cb.HunterId == ids.HunterId)
            .Match(
                _ => ids,
                errors => Failure<(Id<Hunter> HunterId, Id<Bug> BugId)>(errors.ToArray())
            );


    private static async Task<Result<(Hunter Hunter, Bug Bug)>> LoadEntitiesAsync(
        (Id<Hunter> HunterId, Id<Bug> BugId) ids,
        BugHunterContext context
    ) =>
        ValuesToObject(
            await context.Hunters.SingleOrFailureAsync(h => h.Id == ids.HunterId),
            await context.Bugs.SingleOrFailureAsync(b => b.Id == ids.BugId),
            (hunter, bug) => (Hunter: hunter, Bug: bug)
        );

    private Result<(Id<Hunter> HunterId, Id<Bug> BugId)> ToTuple(CatchBugRequest request) =>
        ValuesToObject(
            Id<Hunter>.FromString(request.HunterId),
            Id<Bug>.FromString(request.BugId),
            (hunterId, bugId) => (HunterId: hunterId, BugId: bugId)
        );


    // {
    //     Result<None> result = await handler.HandleAsync(new CatchBugCommand(request.BugId, request.HunterId));
    //     return result.IsSuccess
    //         ? Ok()
    //         : BadRequest(result.Errors);
    // }
}

public record CatchBugRequest(string BugId, string HunterId);