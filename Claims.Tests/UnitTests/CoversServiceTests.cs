using Claims.Application.Dtos;
using Claims.Application.Enums;
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

public class CoversServiceTests
{
    private readonly Mock<ICosmosDbService> _cosmosDbServiceMock;
    private readonly Mock<IAuditer> _auditerMock;
    private readonly Mock<IBackgroundJobClient> _jobClientMock;

    private readonly CoversService _coversService;

    public CoversServiceTests()
    {
        _cosmosDbServiceMock = new Mock<ICosmosDbService>();
        _auditerMock = new Mock<IAuditer>();
        _jobClientMock = new Mock<IBackgroundJobClient>();

        _coversService = new CoversService(_cosmosDbServiceMock.Object, _auditerMock.Object, _jobClientMock.Object);
    }

    [Theory]
    [InlineData("2024-01-01", "2024-03-30", CoverType.Yacht, 118318.75)]
    [InlineData("2024-01-01", "2024-01-31", CoverType.PassengerShip, 45000)]
    [InlineData("2024-01-01", "2024-03-31", CoverType.Tanker, 166500)]
    [InlineData("2024-01-01", "2024-11-30", CoverType.BulkCarrier, 535372.50)]
    public void ComputePremiumInternal_ReturnsCorrectPremium(string startDate, string endDate, CoverType coverType, decimal expectedPremium)
    {
        var startDateObj = DateOnly.Parse(startDate);
        var endDateObj = DateOnly.Parse(endDate);

        var result = _coversService.ComputePremium(startDateObj, endDateObj, coverType);

        result.Should().BeApproximately(expectedPremium, 2);
    }

    [Fact]
    public async Task GetAsync_ReturnsCovers()
    {
        _cosmosDbServiceMock.Setup(x => x.GetAsync<Cover>()).ReturnsAsync(new List<Cover> { new() });

        var result = await _coversService.GetAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAsync_ById_ReturnsCover()
    {
        _cosmosDbServiceMock.Setup(x => x.GetAsync<Cover>(It.IsAny<string>())).ReturnsAsync(new Cover());

        var result = await _coversService.GetAsync("testId");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_CreatesClaim()
    {
        var coverDto = new CoverDto();
        _cosmosDbServiceMock.Setup(x => x.AddItemAsync(It.IsAny<Cover>())).Returns(Task.CompletedTask);

        var result = await _coversService.CreateAsync(coverDto);

        using (new AssertionScope())
        {
            _cosmosDbServiceMock.Verify(x => x.AddItemAsync(It.IsAny<Cover>()), Times.Once);
            _jobClientMock.Verify(x => x.Create(It.Is<Job>(job => job.Method.Name == "AuditCover"), It.IsAny<EnqueuedState>()), Times.Once);
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.ItemType.Should().BeEquivalentTo("Cover");
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesClaim()
    {
        var coverId = "testId";
        _cosmosDbServiceMock.Setup(x => x.DeleteItemAsync<Cover>(It.IsAny<string>())).Returns(Task.CompletedTask);

        await _coversService.DeleteAsync(coverId);

        using (new AssertionScope())
        {
            _cosmosDbServiceMock.Verify(x => x.DeleteItemAsync<Cover>(coverId), Times.Once);
            _jobClientMock.Verify(x => x.Create(It.Is<Job>(job => job.Method.Name == "AuditCover"), It.IsAny<EnqueuedState>()), Times.Once);
        }
    }
}