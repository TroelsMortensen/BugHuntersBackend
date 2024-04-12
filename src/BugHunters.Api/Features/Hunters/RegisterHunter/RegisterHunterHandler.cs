using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Persistence;

namespace BugHunters.Api.Features.Hunters.RegisterHunter;

public class RegisterHunterHandler(BugHunterContext context) : ICommandHandler<RegisterHunterCommand>
{
    public async Task<Result<None>> HandleAsync(RegisterHunterCommand command)
    {
        return Result.Success();
    }
}