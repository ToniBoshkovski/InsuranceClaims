using Claims.Application.Dtos;
using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Moq;
using Xunit;

namespace Claims.Tests.UnitTests;

public class ClaimsServiceTests
{
    private readonly Mock<ICosmosDbService> _cosmosDbServiceMock;
    private readonly Mock<IAuditer> _auditerMock;
    private readonly Mock<IBackgroundJobClient> _jobClientMock;

    private readonly ClaimsService _claimsService;

    public ClaimsServiceTests()
    {
        _cosmosDbServiceMock = new Mock<ICosmosDbService>();
        _auditerMock = new Mock<IAuditer>();
        _jobClientMock = new Mock<IBackgroundJobClient>();

        _claimsService = new ClaimsService(_cosmosDbServiceMock.Object, _auditerMock.Object, _jobClientMock.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsClaims()
    {
        _cosmosDbServiceMock.Setup(x => x.GetAsync<Claim>()).ReturnsAsync(new List<Claim> { new() });

        var result = await _claimsService.GetAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAsync_ById_ReturnsClaim()
    {
        _cosmosDbServiceMock.Setup(x => x.GetAsync<Claim>(It.IsAny<string>())).ReturnsAsync(new Claim());

        var result = await _claimsService.GetAsync("testId");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_CreatesClaim()
    {
        var claimDto = new ClaimDto();
        _cosmosDbServiceMock.Setup(x => x.AddItemAsync(It.IsAny<Claim>())).Returns(Task.CompletedTask);

        var result = await _claimsService.CreateAsync(claimDto);

        using (new AssertionScope())
        {
            _cosmosDbServiceMock.Verify(x => x.AddItemAsync(It.IsAny<Claim>()), Times.Once);
            _jobClientMock.Verify(x => x.Create(It.Is<Job>(job => job.Method.Name == "AuditClaim"), It.IsAny<EnqueuedState>()), Times.Once);
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.ItemType.Should().BeEquivalentTo("Claim");
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesClaim()
    {
        var claimId = "testId";
        _cosmosDbServiceMock.Setup(x => x.DeleteItemAsync<Claim>(It.IsAny<string>())).Returns(Task.CompletedTask);

        await _claimsService.DeleteAsync(claimId);

        using (new AssertionScope())
        {
            _cosmosDbServiceMock.Verify(x => x.DeleteItemAsync<Claim>(claimId), Times.Once);
            _jobClientMock.Verify(x => x.Create(It.Is<Job>(job => job.Method.Name == "AuditClaim"), It.IsAny<EnqueuedState>()), Times.Once);
        }
    }
}