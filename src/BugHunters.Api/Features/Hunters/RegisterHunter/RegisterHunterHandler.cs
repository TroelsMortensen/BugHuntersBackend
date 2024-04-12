using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Persistence;

namespace BugHunters.Api.Features.Hunters.RegisterHunter;

public class RegisterHunterHandler(BugHunterContext context) : ICommandHandler<RegisterHunterCommand>
{
    public async Task<Result<None>> HandleAsync(RegisterHunterCommand command)
    {
        Result<HunterId> hunterIdResult = HunterId.FromString(command.Id);
        Result<ViaId> viaIdResult = ViaId.FromString(command.ViaId);

        Result<None> combined = Result.CombineResultsInto<None>(hunterIdResult, viaIdResult);
        if (combined.IsFailure)
        {
            return combined;
        }

        Hunter hunter = new (hunterIdResult.Payload, command.Name, viaIdResult.Payload);

        await context.Hunters.AddAsync(hunter);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}