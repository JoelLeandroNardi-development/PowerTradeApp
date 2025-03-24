using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PowerTradeCore;

public class CsvExtractionWorker(IPowerService powerService, IConfiguration configuration) : BackgroundService
{
    private readonly IPowerService _powerService = powerService ?? throw new ArgumentNullException(nameof(powerService));
    private readonly int _intervalMinutes = configuration.GetValue<int>("IntervalMinutes", 5);
    private readonly string _folderPath = configuration.GetValue<string>("FolderPath") ?? "../extracts/";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("CSV Extraction Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessCsvExtractionAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:O}] ERROR: {ex.Message}");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Retry after 1 minute
            }

            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);
        }

        Console.WriteLine("CSV Extraction Worker stopped.");
    }

    private async Task ProcessCsvExtractionAsync(CancellationToken stoppingToken)
    {
        string filePath = Path.Combine(_folderPath, CsvGenerator.GenerateCsvFileName(DateTime.UtcNow));
        Directory.CreateDirectory(_folderPath);

        var data = await PowerPositionService.GetAggregatedPositionsAsync(_powerService, DateTime.UtcNow);

        await SaveCsvToFileAsync(filePath, CsvGenerator.GenerateCsvContent(data), stoppingToken);
        Console.WriteLine($"[{DateTime.UtcNow:O}] CSV saved: {filePath}");
    }

    private static async Task SaveCsvToFileAsync(string filePath, string content, CancellationToken cancellationToken) =>
        await File.WriteAllTextAsync(filePath, content, cancellationToken);
}
