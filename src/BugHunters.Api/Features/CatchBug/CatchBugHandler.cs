using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Persistence;

namespace BugHunters.Api.Features.CatchBug;

public class CatchBugHandler(BugHunterContext context) : ICommandHandler<CatchBugCommand>
{
    public async Task<Result<None>> HandleAsync(CatchBugCommand command)
    {
        // Id<Hunter>
        return null;
    }
}