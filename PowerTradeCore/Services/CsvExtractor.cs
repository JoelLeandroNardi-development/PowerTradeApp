namespace PowerTradeCore;

public class CsvExtractor(IPowerPositionService powerPositionService, ICsvGenerator csvGenerator) : ICsvExtractor
{
    public async Task ProcessCsvExtractionAsync(string folderPath, CancellationToken stoppingToken)
    {
        string filePath = Path.Combine(folderPath, csvGenerator.GenerateCsvFileName(DateTime.UtcNow));
        Directory.CreateDirectory(folderPath);

        var data = await powerPositionService.GetAggregatedPositionsAsync(DateTime.UtcNow);

        await SaveCsvToFileAsync(filePath, csvGenerator.GenerateCsvContent(data), stoppingToken);
        Console.WriteLine($"[{DateTime.UtcNow:O}] CSV saved: {filePath}");
    }

    private static async Task SaveCsvToFileAsync(string filePath, string content, CancellationToken cancellationToken) =>
        await File.WriteAllTextAsync(filePath, content, cancellationToken);
}
