using BugHunters.Api.Common.Result;
using BugHunters.Api.Entities;
using BugHunters.Api.Features.RegisterHunter;

namespace UnitTests.Services;

public class CreateHunterServiceTest
{
    [Theory]
    [MemberData(nameof(GenerateValidInput))]
    public void CreateHunter_ValidInput_ReturnSuccess(string id, string name, string viaId)
    {
        // Arrange
        CreateHunterService service = new();

        // Act
        Result<Hunter> result = service.CreateHunter(id, name, viaId);
        Hunter hunter = result.Payload;
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(id, hunter.Id.Value.ToString());
        Assert.Equal(name, hunter.DisplayName.Value);
        Assert.Equal(viaId, hunter.ViaId.Value);
    }


    public static IEnumerable<object[]> GenerateValidInput()
    {
        string[] guids =
        [
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        ];
        string[] names = ["John Doe", "Jane Doe", "John Smith", "A", "hfdasjfkhsfjdhkasdfa"];
        string[] viaIds =
        [
            "250312",
            "330512",
            "310742",
            "290311",
            "trmo",
            "iha",
            "mivi",
            "jknr"
        ];

        return StringArraysToObjectArray(guids, names, viaIds);
    }

    private static IEnumerable<object[]> StringArraysToObjectArray(string[] guids, string[] names, string[] viaIds)
        => from guid in guids
            from name in names
            from viaId in viaIds
            select new object[] { guid, name, viaId };

    [Theory]
    [MemberData(nameof(GenerateInvalidInput))]
    public void CreateHunter_InvalidInput_ReturnFailure(string id, string name, string viaId)
    {
        // Arrange
        CreateHunterService service = new();

        // Act
        Result<Hunter> result = service.CreateHunter(id, name, viaId);

        // Assert
        Assert.True(result.IsFailure);
    }

    public static IEnumerable<object[]> GenerateInvalidInput()
    {
        string[] guids =
        [
            "",
            "invalidGuid"
        ];
        string[] names = ["", "absrhjerdfhjshjadfhjk"];
        string[] viaIds =
        [
            "25031",
            "3312",
            "342",
            "2",
            "",
            "tr",
            "i",
            "mivid",
            "jknrfjdk",
            "123kjn",
            "jf2"
        ];
        return StringArraysToObjectArray(guids, names, viaIds);
    }
}