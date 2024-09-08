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
            .Where(HunterExists(context))
            .Where(BugExists())
            .Where(BugNotAlreadyCaught(context))
            .Map(IdsToBugCatch)
            .Map(AddBugCatchToDb())
            .Tee(Save())
            .Match(
                _ => Results.Ok(),
                ToProblemDetails);

    private Func<Task<Result<None>>> Save() =>
        context.TrySaveChangesAsync;

    private Func<BugCatch, BugCatch> AddBugCatchToDb() =>
        bugCatch => context.BugCatches.Add(bugCatch).Entity;

    private Func<HunterBugIds, Task<Result<HunterBugIds>>> BugExists() =>
        ids => context.Bugs.SingleOrFailureAsync(b => b.Id == ids.BugId)
            .Match(
                _ => ids,
                errors => Failure<HunterBugIds>(errors.ToArray())
            );

    private static Func<HunterBugIds, Task<Result<HunterBugIds>>> BugNotAlreadyCaught(BugHunterContext ctx) =>
        ids => ctx.BugCatches.SingleOrFailureAsync(cb => cb.BugId == ids.BugId && cb.HunterId == ids.HunterId)
            .Match(
                _ => ids,
                errors => Failure<HunterBugIds>(errors.ToArray())
            );

    private static BugCatch IdsToBugCatch(HunterBugIds tuple) =>
        new(tuple.HunterId, tuple.BugId, DateTime.Now);

    private static Func<HunterBugIds, Task<Result<HunterBugIds>>> HunterExists(BugHunterContext ctx) =>
        ids => ctx.Hunters.SingleOrFailureAsync(h => h.Id == ids.HunterId)
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