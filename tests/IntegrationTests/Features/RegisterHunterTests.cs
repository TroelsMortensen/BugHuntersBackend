using System.Net;
using BugHunters.Api.Common.Result;
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
        RegisterResponseCopy? response =
            await httpResponse.Content.ReadFromJsonAsync<RegisterResponseCopy>();

        Assert.NotNull(response);
        Assert.NotEmpty(response.HunterId);
    }
    
    private record RegisterResponseCopy(string HunterId, string HunterName, string ViaId);


    [Fact]
    public async Task RegisterHunter_InvalidName_ShouldReturnBadRequest()
    {
        RegisterHunterEndpoint.RegisterRequest content = new("", "trmo");

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));

        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
    }

    [Fact]
    public async Task RegisterHunter_InvalidViaId_ShouldReturnBadRequest()
    {
        RegisterHunterEndpoint.RegisterRequest content = new("John Doe", "tr");

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));

        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData("J")]
    [InlineData("absjkdlfjdkjkljefklja")]
    public async Task RegisterHunter_InvalidName_ShouldReturnBadRequestWithErrorMessage(string invalidName)
    {
        RegisterHunterEndpoint.RegisterRequest content = new(invalidName, "trmo");

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));
        List<ResultError>? response = await httpResponse.Content.ReadFromJsonAsync<List<ResultError>>();

        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Contains(response, error => error.Code.Equals("Hunter.Name"));
    }

    [Theory]
    [InlineData("tr")]
    [InlineData("")]
    [InlineData("tr12")]
    [InlineData("troel")]
    [InlineData("12345")]
    [InlineData("1234567")]
    public async Task RegisterHunter_InvalidViaId_ShouldReturnBadRequestWithErrorMessage(string invalidViaId)
    {
        RegisterHunterEndpoint.RegisterRequest content = new("John Doe", invalidViaId);

        HttpResponseMessage httpResponse = await client.PostAsync("/api/register-hunter", JsonContent.Create(content));
        List<ResultError>? response = await httpResponse.Content.ReadFromJsonAsync<List<ResultError>>();

        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Contains(response, error => error.Code.Equals("Hunter.ViaId"));
    }
}