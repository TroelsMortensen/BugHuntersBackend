using System.Net;
using BugHunters.Api.Entities;
using BugHunters.Api.Entities.Values;
using BugHunters.Api.Features.Hunters.RegisterHunter;
using BugHunters.Api.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Features;

public class RegisterHunterTests
{
    [Fact]
    public async Task ShouldRegisterHunter()
    {
        // Arrange
        await using WebApplicationFactory<Program> webAppFac = new BugHunterWebAppFactory();
        HttpClient client = webAppFac.CreateClient();

        RegisterHunterEndpoint.RegisterRequest content = new("Troels", "trmo");
        
        // Act
        HttpResponseMessage httpResponse = await client.PostAsync("/api/hunters/register", JsonContent.Create(content));
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        
        RegisterHunterEndpoint.RegisterResponse response = (await httpResponse.Content.ReadFromJsonAsync<RegisterHunterEndpoint.RegisterResponse>())!;
        var id = Id<Hunter>.FromString(response.Id).EnsureValidResult().Payload;
        
        await using BugHunterContext context = webAppFac.Services.CreateScope().ServiceProvider.GetRequiredService<BugHunterContext>();
        Hunter hunter = context.Hunters.Single(h => h.Id == id);
        Assert.NotNull(hunter);
    }
}