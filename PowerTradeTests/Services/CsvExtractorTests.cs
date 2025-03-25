﻿using Microsoft.Extensions.Logging;
using Moq;
using PowerTradeCore;

namespace PowerTradeTests;

public class CsvExtractorTests
{
    private readonly Mock<IPowerPositionService> _mockPowerPositionService;
    private readonly Mock<ICsvGenerator> _mockCsvGenerator;
    private readonly Mock<ILogger<CsvExtractor>> _mockLogger;
    private readonly CsvExtractor _csvExtractor;
    private readonly string _testFolderPath = "./test_output";

    public CsvExtractorTests()
    {
        _mockPowerPositionService = new Mock<IPowerPositionService>();
        _mockCsvGenerator = new Mock<ICsvGenerator>();
        _mockLogger = new Mock<ILogger<CsvExtractor>>();
        _csvExtractor = new CsvExtractor(_mockPowerPositionService.Object, _mockCsvGenerator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ProcessCsvExtractionAsync_ShouldCreateDirectoryAndSaveCsv()
    {
        // Arrange
        var testData = TestDataProvider.TestData;
        string testFileName = "PowerPosition_20240325_1200.csv";
        string testCsvContent = "Test CSV Content";
        SetupMockServices(testData, testFileName, testCsvContent);
        var cancellationToken = new CancellationToken();

        // Act
        await _csvExtractor.ProcessCsvExtractionAsync(_testFolderPath, cancellationToken);

        // Assert
        string expectedFilePath = Path.Combine(_testFolderPath, testFileName);
        Assert.True(File.Exists(expectedFilePath), "CSV file was not created.");
        string fileContent = await File.ReadAllTextAsync(expectedFilePath);
        Assert.Equal(testCsvContent, fileContent);
        File.Delete(expectedFilePath);
    }

    [Fact]
    public async Task ProcessCsvExtractionAsync_ShouldLogInformation()
    {
        // Arrange
        var testData = TestDataProvider.TestData;
        string testFileName = "PowerPosition_20240325_1200.csv";
        string testCsvContent = "Test CSV Content";
        SetupMockServices(testData, testFileName, testCsvContent);
        var cancellationToken = new CancellationToken();

        // Act
        await _csvExtractor.ProcessCsvExtractionAsync(_testFolderPath, cancellationToken);

        // Assert
        VerifyLogMessage(_mockLogger, "Requesting data from PowerPosition service.", LogLevel.Information);
        VerifyLogMessage(_mockLogger, "CSV saved:", LogLevel.Information);
    }

    private void SetupMockServices(IEnumerable<AccumulatedPowerTrade> testData, string testFileName, string testCsvContent)
    {
        _mockPowerPositionService
            .Setup(x => x.GetAggregatedPositionsAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(testData);

        _mockCsvGenerator
            .Setup(x => x.GenerateCsvFileName(It.IsAny<DateTime>()))
            .Returns(testFileName);

        _mockCsvGenerator
            .Setup(x => x.GenerateCsvContent(testData))
            .Returns(testCsvContent);
    }

    private void VerifyLogMessage(Mock<ILogger<CsvExtractor>> logger, string expectedMessage, LogLevel logLevel) =>
        logger.Verify(x => x.Log(
            logLevel,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
}
