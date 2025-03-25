using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PowerTradeCore;

namespace PowerTradeTests;

public class CsvExtractionWorkerTests
{
    private readonly Mock<ICsvExtractor> _mockCsvExtractor;
    private readonly Mock<ILogger<CsvExtractionWorker>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly CsvExtractionWorker _subject;

    public CsvExtractionWorkerTests()
    {
        _mockCsvExtractor = new Mock<ICsvExtractor>();
        _mockLogger = new Mock<ILogger<CsvExtractionWorker>>();
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { Constants.IntervalMinutes, "5" },
                { Constants.FolderPath, "../extracts/" }
            }!)
            .Build();
        _subject = new CsvExtractionWorker(_mockCsvExtractor.Object, _configuration, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallProcessCsvExtractionAsync()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        // Act
        await _subject.StartAsync(cancellationTokenSource.Token);

        // Assert
        _mockCsvExtractor.Verify(x => x.ProcessCsvExtractionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
