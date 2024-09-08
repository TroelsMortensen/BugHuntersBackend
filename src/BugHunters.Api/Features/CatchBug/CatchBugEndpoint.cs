using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugEndpoint(BugHunterContext context)
    : ApiEndpoint.WithRequest<CatchBugEndpoint.CatchBugRequest>
{
    [HttpPost("catch-bug")]
    public override async Task<IResult> HandleAsync(CatchBugRequest request) =>
        await ToIdPairFrom(request)
            .Where(idPair => BugNotAlreadyCaught(idPair, context))
            .Where(idPair => HunterExists(idPair, context))
            .Where(idPair => BugExists(idPair, context))
            .Map(IdsToBugCatch)
            .Map(bugCatch => context.BugCatches.Add(bugCatch).Entity)
            .Tee(context.TrySaveChangesAsync)
            .Match(
                _ => Results.Ok(),
                ToProblemDetails);


    private static BugCatch IdsToBugCatch(HunterBugIds tuple) =>
        new(tuple.HunterId, tuple.BugId, DateTime.Now);

    private static async Task<Result<HunterBugIds>> BugExists(
        HunterBugIds tuple,
        BugHunterContext ctx) =>
        await ctx.Bugs.SingleOrFailureAsync(b => b.Id == tuple.BugId)
            .Match(
                _ => tuple,
                errors => Failure<HunterBugIds>(errors.ToArray())
            );


    private static async Task<Result<HunterBugIds>> HunterExists(
        HunterBugIds ids,
        BugHunterContext ctx) =>
        await ctx.Hunters.SingleOrFailureAsync(h => h.Id == ids.HunterId)
            .Match(
                _ => ids,
                errors => Failure<HunterBugIds>(errors.ToArray())
            );

    private static async Task<Result<HunterBugIds>> BugNotAlreadyCaught(
        HunterBugIds ids,
        BugHunterContext ctx
    ) =>
        await ctx.BugCatches.SingleOrFailureAsync(cb => cb.BugId == ids.BugId && cb.HunterId == ids.HunterId)
            .Match(
                _ => ids,
                errors => Failure<HunterBugIds>(errors.ToArray())
            );

    private static Result<HunterBugIds> ToIdPairFrom(CatchBugRequest request) =>
        ValuesToObject(
            Id<Hunter>.FromString(request.HunterId),
            Id<Bug>.FromString(request.BugId),
            (hunterId, bugId) => new HunterBugIds(hunterId, bugId)
        );

    private record HunterBugIds(Id<Hunter> HunterId, Id<Bug> BugId);

    public record CatchBugRequest(string BugId, string HunterId);
}