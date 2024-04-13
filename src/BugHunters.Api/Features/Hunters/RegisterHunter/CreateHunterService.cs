using BugHunters.Api.Common.FunctionalCore;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Entities.Values.Hunter;

namespace BugHunters.Api.Features.Hunters.RegisterHunter;

public class CreateHunterService : ICoreService
{
    public Result<Hunter> CreateHunter(string id, string name, string viaId)
    {
        Result<HunterId> hunterIdResult = HunterId.FromString(id);
        Result<ViaId> viaIdResult = ViaId.FromString(viaId);
        Result<Name> nameResult = Name.FromString(name);
        Result<Hunter> result = Result
            .CombineResultsInto<Hunter>(hunterIdResult, viaIdResult, nameResult)
            .WithPayloadIfSuccess(
                () => new Hunter(
                    hunterIdResult.Payload,
                    nameResult.Payload,
                    viaIdResult.Payload
                )
            );
        return result;
    }
}