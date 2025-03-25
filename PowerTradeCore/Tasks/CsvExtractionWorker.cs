using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace PowerTradeCore;

public class CsvExtractionWorker(ICsvExtractor csvExtractor,
                                 IConfiguration configuration,
                                 ILogger<CsvExtractionWorker> logger) : BackgroundService
{
    private readonly int _intervalMinutes = configuration.GetValue(Constants.IntervalMinutes, 5);
    private readonly string _folderPath = configuration.GetValue<string>(Constants.FolderPath) ?? "../extracts/";
    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(30));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CSV Extraction Worker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            await _retryPolicy.ExecuteAsync(async () => await csvExtractor.ProcessCsvExtractionAsync(_folderPath, stoppingToken));
            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);
        }
        logger.LogInformation("CSV Extraction Worker stopped.");
    }
}
