using BugHunters.Api.Features.RegisterHunter;

namespace IntegrationTests;

public abstract class TestBase
{
    internal BugHunterWebAppFactory factory;
    internal HttpClient client => factory.CreateClient();

    protected TestBase()
    {
        factory = new BugHunterWebAppFactory();
    }

}