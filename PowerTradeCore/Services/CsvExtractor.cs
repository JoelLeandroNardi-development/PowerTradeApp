using Microsoft.Extensions.Logging;

namespace PowerTradeCore;

public class CsvExtractor(IPowerPositionService powerPositionService, ICsvGenerator csvGenerator, ILogger<CsvExtractor> logger) : ICsvExtractor
{
    public async Task ProcessCsvExtractionAsync(string folderPath, CancellationToken stoppingToken)
    {
        string filePath = Path.Combine(folderPath, csvGenerator.GenerateCsvFileName(DateTime.UtcNow));
        Directory.CreateDirectory(folderPath);

        logger.LogInformation("Requesting data from PowerPosition service.");
        var data = await powerPositionService.GetAggregatedPositionsAsync(DateTime.UtcNow);

        await SaveCsvToFileAsync(filePath, csvGenerator.GenerateCsvContent(data), stoppingToken);
        logger.LogInformation($"[{DateTime.UtcNow:O}] CSV saved: {filePath}");
    }

    private static async Task SaveCsvToFileAsync(string filePath, string content, CancellationToken cancellationToken) =>
        await File.WriteAllTextAsync(filePath, content, cancellationToken);
}
