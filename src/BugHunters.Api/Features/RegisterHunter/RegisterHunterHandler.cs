using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Entities;
using BugHunters.Api.Persistence;

namespace BugHunters.Api.Features.RegisterHunter;

public class RegisterHunterHandler(BugHunterContext context, CreateHunterService service) : ICommandHandler<RegisterHunterCommand>
{
    public async Task<Result<None>> HandleAsync(RegisterHunterCommand command)
    {
        Result<Hunter> hunterResult = service.CreateHunter(
            command.Id,
            command.Name,
            command.ViaId);
        
        if (hunterResult.IsFailure)
        {
            return hunterResult.Errors.ToList();
        }

        Hunter hunter = hunterResult.Payload;

        await context.Hunters.AddAsync(hunter);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}