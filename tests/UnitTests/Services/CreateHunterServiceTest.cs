using BugHunters.Api.Common.Result;
using BugHunters.Api.Entities;
using BugHunters.Api.Features.RegisterHunter;

namespace UnitTests.Services;

public abstract class CreateHunterServiceTest
{
    public abstract class CreateHunter
    {
        public class Succeeds
        {
            [Theory]
            [MemberData(nameof(ValidIds))]
            public void GivenValidHunterId(string validId)
            {
                const string validName = "John Doe";
                const string validViaId = "250312";

                ExecuteSuccessScenario(validId, validName, validViaId);
            }

            [Theory]
            [MemberData(nameof(ValidNames))]
            public void GivenValidName(string validName)
            {
                string validId = Guid.NewGuid().ToString();
                const string validViaId = "250312";

                ExecuteSuccessScenario(validId, validName, validViaId);
            }

            [Theory]
            [MemberData(nameof(ValidViaIds))]
            public void GivenValidViaId(string validViaId)
            {
                string validId = Guid.NewGuid().ToString();
                const string validName = "John Doe";

                ExecuteSuccessScenario(validId, validName, validViaId);
            }

            private static void ExecuteSuccessScenario(string validId, string validName, string validViaId)
            {
                // Arrange
                CreateHunterService service = new();

                // Act
                Result<Hunter> result = service.CreateHunter(validId, validName, validViaId);
                Hunter hunter = result.Match(
                    value => value,
                    errors => throw new Exception("Should not happen.")
                );
                // Assert
                Assert.True(result is Success<Hunter>);
                Assert.Equal(validId, hunter.Id.Value.ToString());
                Assert.Equal(validName, hunter.DisplayName.Value);
                Assert.Equal(validViaId, hunter.ViaId.Value);
            }

            public static IEnumerable<object[]> ValidIds()
                =>
                [
                    [Guid.NewGuid().ToString()],
                    [Guid.NewGuid().ToString()],
                    [Guid.NewGuid().ToString()],
                ];

            public static IEnumerable<object[]> ValidNames()
                =>
                [
                    ["John Doe"],
                    ["Jane Doe"],
                    ["John Smith"],
                    ["A"],
                    ["hfdasjfkhsfjdhkasdfa"]
                ];

            public static IEnumerable<object[]> ValidViaIds()
                =>
                [
                    ["250312"],
                    ["330512"],
                    ["310742"],
                    ["290311"],
                    ["trmo"],
                    ["iha"],
                    ["mivi"],
                    ["jknr"]
                ];
        }
    }

    // [Theory]
    // [MemberData(nameof(GenerateValidInput))]
    // public void CreateHunter_ValidInput_ReturnSuccess(string id, string name, string viaId)
    // {
    //     // Arrange
    //     CreateHunterService service = new();
    //
    //     // Act
    //     Result<Hunter> result = service.CreateHunter(id, name, viaId);
    //     Hunter hunter = result.Payload;
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(id, hunter.Id.Value.ToString());
    //     Assert.Equal(name, hunter.DisplayName.Value);
    //     Assert.Equal(viaId, hunter.ViaId.Value);
    // }
    //
    //
    // public static IEnumerable<object[]> GenerateValidInput()
    // {
    //     string[] guids =
    //     [
    //         Guid.NewGuid().ToString(),
    //         Guid.NewGuid().ToString(),
    //         Guid.NewGuid().ToString()
    //     ];
    //     string[] names = ["John Doe", "Jane Doe", "John Smith", "A", "hfdasjfkhsfjdhkasdfa"];
    //     string[] viaIds =
    //     [
    //         "250312",
    //         "330512",
    //         "310742",
    //         "290311",
    //         "trmo",
    //         "iha",
    //         "mivi",
    //         "jknr"
    //     ];
    //
    //     return StringArraysToObjectArray(guids, names, viaIds);
    // }
    //
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
        Assert.True(result is Failure<Hunter>);
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