using System.Net;
using BugHunters.Api.Features.RegisterHunter;

namespace IntegrationTests.Features;

public class RegisterHunterTests : TestBase
{
    [Fact]
    public async Task RegisterHunter_ValidHunter_ShouldReturnOk()
    {
        RegisterHunterEndpoint.RegisterRequest content = CreateValidRegisterRequest();

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }

    [Fact]
    public async Task RegisterHunter_ValidHunter_ShouldReturnValidHunterId()
    {
        RegisterHunterEndpoint.RegisterRequest content = CreateValidRegisterRequest();

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));
        RegisterHunterEndpoint.RegisterResponse? response = await httpResponse.Content.ReadFromJsonAsync<RegisterHunterEndpoint.RegisterResponse>();
        
        Assert.NotNull(response);
        Assert.NotEmpty(response.Id);
    }

    [Fact]
    public async Task RegisterHunter_InvalidName_ShouldReturnBadRequest()
    {
    }

    [Fact]
    public async Task RegisterHunter_InvalidViaId_ShouldReturnBadRequest()
    {
    }
}