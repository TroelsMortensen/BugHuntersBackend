using BugHunters.Api.Common.Endpoint;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Common;
using BugHunters.Api.Entities.HunterEntity;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BugHunters.Api.Common.Result.ResultExt;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugEndpoint(BugHunterContext context)
    : ApiEndpoint.WithRequest<CatchBugEndpoint.CatchBugRequest>
{
    [HttpPost("catch-bug")]
    public override async Task<IResult> HandleAsync(CatchBugRequest request) =>
        await ToIdPairFrom(request)
            .Where(ids => context.HunterExists(ids.HunterId, ids))
            .Where(BugExists(context))
            .Where(BugNotAlreadyCaught(context))
            .Map(IdsToBugCatch)
            .Map(AddBugCatchToDb(context))
            .Tee(Save(context))
            .Match(
                _ => Results.NoContent(),
                ToProblemDetails);

    private static Func<Task<Result<None>>> Save(BugHunterContext ctx) =>
        ctx.TrySaveChangesAsync;

    private static Func<BugCatch, BugCatch> AddBugCatchToDb(BugHunterContext ctx) =>
        bugCatch => ctx.BugCatches.Add(bugCatch).Entity;

    private static Func<HunterBugIds, Task<Result<HunterBugIds>>> BugExists(BugHunterContext ctx) =>
        async ids => await ctx.Bugs.AnyAsync(b => b.Id == ids.BugId)
            ? Success(ids)
            : Failure<HunterBugIds>(new ResultError("BugNotFound", "Bug not found."));

    private static Func<HunterBugIds, Task<Result<HunterBugIds>>> BugNotAlreadyCaught(BugHunterContext ctx) =>
        async ids => await ctx.BugCatches.AnyAsync(cb => cb.BugId == ids.BugId && cb.HunterId == ids.HunterId)
            ? Success(ids)
            : Failure<HunterBugIds>(new ResultError("BugCatch", "Bug already caught."));


    private static BugCatch IdsToBugCatch(HunterBugIds tuple) =>
        new(tuple.HunterId, tuple.BugId, DateTime.Now);

    // private static Func<HunterBugIds, Task<Result<HunterBugIds>>> HunterExists(BugHunterContext ctx) =>
    //     async ids => await ctx.Hunters.AnyAsync(h => h.Id == ids.HunterId)
    //         ? Success(ids)
    //         : Failure<HunterBugIds>(new ResultError("HunterNotFound", "Hunter not found."));


    private static Result<HunterBugIds> ToIdPairFrom(CatchBugRequest request) =>
        ValuesToObject(
            Id<Hunter>.FromString(request.HunterId),
            Id<Bug>.FromString(request.BugId),
            (hunterId, bugId) => new HunterBugIds(hunterId, bugId)
        );

    private record HunterBugIds(Id<Hunter> HunterId, Id<Bug> BugId);

    public record CatchBugRequest(string BugId, string HunterId);
}