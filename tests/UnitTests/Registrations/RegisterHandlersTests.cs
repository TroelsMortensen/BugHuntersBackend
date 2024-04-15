using BugHunters.Api.Common.FunctionalCore;
using BugHunters.Api.Common.HandlerContract;
using BugHunters.Api.Features.Hunters.RegisterHunter;
using BugHunters.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Registrations;

public class RegisterHandlersTests
{
    [Fact]
    public void HandlersAreRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterCommandHandlers();
        services.RegisterCoreServices();
        services.AddDbContext<BugHunterContext>(options => { options.UseSqlite(@"Data Source = Test.db"); });

        ServiceProvider provider = services.BuildServiceProvider();
        IServiceScope serviceScope = provider.CreateScope();
        // Assert
        ICommandHandler<RegisterHunterCommand>? handler = serviceScope.ServiceProvider.GetService<ICommandHandler<RegisterHunterCommand>>();
        Assert.NotNull(handler);
    }
}