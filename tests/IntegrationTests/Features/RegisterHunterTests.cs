using BugHunters.Api.Persistence;

namespace IntegrationTests.Features;

public class RegisterHunterTests
{
    [Fact]
    public async Task ShouldRegisterHunter()
    {
        // Arrange
        await using BugHunterContext context = DbUtil.SetupContext();
    }
}