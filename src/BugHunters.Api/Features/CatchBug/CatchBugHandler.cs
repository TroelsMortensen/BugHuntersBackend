using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugHandler(BugHunterContext context) : ICommandHandler<CatchBugCommand>
{
    public async Task<Result<None>> HandleAsync(CatchBugCommand command)
    {
        Result<Id<Hunter>> hunterId = Id<Hunter>.FromString(command.HunterId);
        Result<Id<Bug>> bugId = Id<Bug>.FromString(command.BugId);
        Result<None> combined = Result.CombineResults(hunterId, bugId);
        
        if (combined.IsFailure)
        {
            return combined;
        }

        Hunter? hunter = await context.Hunters.SingleOrDefaultAsync(h => h.Id == hunterId.Payload);
        if (hunter is null)
        {
            return new ResultError("Hunter.Id", $"Hunter with id {command.HunterId} not found");
        }
        
        Bug? bug = await context.Bugs.SingleOrDefaultAsync(b => b.Id == bugId.Payload);
        if (bug is null)
        {
            return new ResultError("Bug.Id", $"Bug with id {command.BugId} not found");
        }
        
        BugCatch bugCatch = new (hunter.Id, bug.Id, DateTime.UtcNow);
        await context.BugCatches.AddAsync(bugCatch);
        await context.SaveChangesAsync();
        
        return Result.Success();
    }
}