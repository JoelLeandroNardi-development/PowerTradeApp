namespace PowerTradeCore;

public interface ICsvExtractor
{
    Task ProcessCsvExtractionAsync(string folderPath, CancellationToken stoppingToken);
}