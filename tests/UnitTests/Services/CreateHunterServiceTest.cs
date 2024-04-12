using BugHunters.Api.Common.Result;
using BugHunters.Api.Entities;
using BugHunters.Api.Features.Hunters.RegisterHunter;

namespace UnitTests.Services;

public class CreateHunterServiceTest
{
    [Theory]
    [MemberData(nameof(GenerateValidInput))]
    public void CreateHunter_ValidInput_ReturnSuccess(string id, string name, string viaId)
    {
        // Arrange
        CreateHunterService service = new ();
        
        // Act
        Result<Hunter> result = service.CreateHunter(id, name, viaId);
        Hunter hunter = result.Payload;
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(id, hunter.Id.Value.ToString());
        Assert.Equal(name, hunter.Name);
        Assert.Equal(viaId, hunter.ViaId.Value.ToString());
    }

    public static IEnumerable<object[]> GenerateValidInput()
    {
        string[] guids =
        [
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        ];
        string[] names = ["John Doe", "Jane Doe", "John Smith"];
        string[] viaIds = [
            "250312",
            "330512",
            "310742",
            "290311",
            "trmo",
            "iha",
            "mivi",
            "jknr"
        ];
        foreach (string guid in guids)
        {
            foreach (string name in names)
            {
                foreach (string viaId in viaIds)
                {
                    yield return [guid, name, viaId];
                }
            }
        }
    }
}