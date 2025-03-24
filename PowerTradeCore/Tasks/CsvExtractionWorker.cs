using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Retry;

namespace PowerTradeCore
{
    public class CsvExtractionWorker(IPowerService powerService, IConfiguration configuration) : BackgroundService
    {
        private readonly IPowerService _powerService = powerService ?? throw new ArgumentNullException(nameof(powerService));
        private readonly int _intervalMinutes = configuration.GetValue(Constants.IntervalMinutes, 5);
        private readonly string _folderPath = configuration.GetValue<string>(Constants.FolderPath) ?? "../extracts/";
        private readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(30));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("CSV Extraction Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await ProcessCsvExtractionAsync(stoppingToken);
                });

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
}
