using Microsoft.Extensions.DependencyInjection;
using PowerTradeCore;

namespace PowerTradeConsole;

public static class Scenario2
{
    public static async Task RunAsync(ServiceProvider serviceProvider)
    {
        var csvExtractor = serviceProvider.GetRequiredService<ICsvExtractor>();
        Console.WriteLine("Enter the folder path where you want to save the CSV:");
        string folderPath = Console.ReadLine()!;
        if (!Path.IsPathRooted(folderPath)) folderPath = Path.Combine(Constants.FromConsolePath, folderPath);
        await csvExtractor.ProcessCsvExtractionAsync(folderPath, new CancellationToken());
    }
}
