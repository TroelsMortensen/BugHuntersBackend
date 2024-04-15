using BugHunters.Api.Common.FunctionalCore;
using BugHunters.Api.Features.RegisterHunter;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Registrations;

public class RegisterCoreServicesTests
{
    [Fact]
    public void CoreServicesAreRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterCoreServices();

        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope serviceScope = provider.CreateScope();
        // Assert
        CreateHunterService? coreService = serviceScope.ServiceProvider.GetService<CreateHunterService>();
        Assert.NotNull(coreService);
    }
}