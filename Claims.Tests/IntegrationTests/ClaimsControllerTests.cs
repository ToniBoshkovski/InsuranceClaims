using AutoFixture;
using Claims.Application.Dtos;
using Claims.Application.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Claims.Tests.IntegrationTests;

public class ClaimsControllerTests()
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task GetAsync_ReturnClaims()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(_ => { });

        var client = application.CreateClient();
        var response = await client.GetAsync("/Claims");
        var content = await response.Content.ReadAsStringAsync();
        var claims = JsonConvert.DeserializeObject<IEnumerable<Claim>>(content);

        using (new AssertionScope())
        {
            response.EnsureSuccessStatusCode();
            response.RequestMessage?.Method.Should().Be(HttpMethod.Get);
            claims.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task CreateAsync_ReturnValidationError()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(_ => { });

        var client = application.CreateClient();

        var claimDto = _fixture.Build<ClaimDto>().Create();

        var content = new StringContent(JsonConvert.SerializeObject(claimDto), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/Claims", content);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.RequestMessage?.Method.Should().Be(HttpMethod.Post);
        }
    }
}