using Microsoft.AspNetCore.Mvc;
using Moq;
using PowerTradeCore;
using System.Text;

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
        var mockData = new List<AccumulatedPowerTrade>
        {
            new(now.ToString(), 100)
        };

        mockPowerPositionService
            .Setup(service => service.GetAggregatedPositionsAsync(now))
            .ReturnsAsync(mockData);

        // Set up mock CSV generation behavior
        var expectedCsv = "Date,Volume\n" + now.ToString("yyyy-MM-dd") + ",100\n"; // Expected CSV format
        mockCsvGenerator.Setup(generator => generator.GenerateCsvContent(mockData)).Returns(expectedCsv);
        mockCsvGenerator.Setup(generator => generator.GenerateCsvFileName(now)).Returns("PowerPositionData.csv");

        // Act
        var result = await GetPowerPositionCsv.GenerateCSVAsync(mockPowerPositionService.Object, mockCsvGenerator.Object);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType); // Assert the content type
        Assert.Equal(expectedCsv, Encoding.UTF8.GetString(fileResult.FileContents)); // Assert the CSV content
        Assert.Equal("PowerPositionData.csv", fileResult.FileDownloadName); // Assert the file name
    }

    [Fact]
    public async Task GenerateCSVAsync_ShouldReturnEmptyFile_WhenNoDataFound()
    {
        // Arrange
        var mockPowerPositionService = new Mock<IPowerPositionService>();
        var mockCsvGenerator = new Mock<ICsvGenerator>();

        var now = DateTime.Now;

        // Set up mock to return no data
        mockPowerPositionService
            .Setup(service => service.GetAggregatedPositionsAsync(now))
            .ReturnsAsync(new List<AccumulatedPowerTrade>());

        // Set up mock CSV generation (empty content)
        mockCsvGenerator.Setup(generator => generator.GenerateCsvContent(It.IsAny<IEnumerable<AccumulatedPowerTrade>>())).Returns(string.Empty);
        mockCsvGenerator.Setup(generator => generator.GenerateCsvFileName(now)).Returns("PowerPositionData.csv");

        // Act
        var result = await GetPowerPositionCsv.GenerateCSVAsync(mockPowerPositionService.Object, mockCsvGenerator.Object);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType); // Assert the content type
        Assert.Empty(fileResult.FileContents); // Assert that the CSV content is empty
        Assert.Equal("PowerPositionData.csv", fileResult.FileDownloadName); // Assert the file name
    }

    [Fact]
    public async Task GenerateCSVAsync_ShouldHandleCsvGenerationError_Gracefully()
    {
        // Arrange
        var mockPowerPositionService = new Mock<IPowerPositionService>();
        var mockCsvGenerator = new Mock<ICsvGenerator>();

        var now = DateTime.Now;

        // Set up mock data to return from PowerPositionService
        var mockData = new List<AccumulatedPowerTrade>
        {
            new(now.ToString(), 100)
        };

        mockPowerPositionService
            .Setup(service => service.GetAggregatedPositionsAsync(now))
            .ReturnsAsync(mockData);

        // Simulate an error in CSV generation
        mockCsvGenerator.Setup(generator => generator.GenerateCsvContent(mockData)).Throws(new Exception("Error generating CSV"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => GetPowerPositionCsv.GenerateCSVAsync(mockPowerPositionService.Object, mockCsvGenerator.Object));
        Assert.Equal("Error generating CSV", exception.Message);
    }
}