using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PowerTradeCore;

namespace PowerTradeTests;

public class GetPowerPositionCsvTests
{
    [Fact]
    public async Task GenerateCSVAsync_ShouldReturnFileResult_WhenCalled()
    {
        // Arrange
        var mockPowerPositionService = new Mock<IPowerPositionService>();
        var mockCsvGenerator = new Mock<ICsvGenerator>();
        var now = DateTime.Now;
        var mockData = TestDataProvider.TestData;
        mockPowerPositionService
            .Setup(service => service.GetAggregatedPositionsAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(mockData);
        var expectedCsv = "Date,Volume\n" + now.ToString("yyyy-MM-dd") + ",100\n"; // Expected CSV format
        mockCsvGenerator.Setup(generator => generator.GenerateCsvContent(mockData)).Returns(expectedCsv);
        mockCsvGenerator.Setup(generator => generator.GenerateCsvFileName(It.IsAny<DateTime>())).Returns("PowerPositionData.csv");

        // Act
        var result = await GetPowerPositionCsv.GenerateCSVAsync(mockPowerPositionService.Object, mockCsvGenerator.Object);

        // Assert
        var fileResult = Assert.IsType<FileContentHttpResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal("PowerPositionData.csv", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task GenerateCSVAsync_ShouldHandleCsvGenerationError_Gracefully()
    {
        // Arrange
        var mockPowerPositionService = new Mock<IPowerPositionService>();
        var mockCsvGenerator = new Mock<ICsvGenerator>();
        var now = DateTime.Now;
        var mockData = TestDataProvider.TestData;
        mockPowerPositionService
            .Setup(service => service.GetAggregatedPositionsAsync(now))
            .ReturnsAsync(mockData);
        mockCsvGenerator.Setup(generator => generator.GenerateCsvContent(mockData)).Throws(new Exception("Error generating CSV"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => GetPowerPositionCsv.GenerateCSVAsync(mockPowerPositionService.Object, mockCsvGenerator.Object));
    }
}
