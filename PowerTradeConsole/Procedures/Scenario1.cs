using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerTradeCore;

namespace PowerTradeConsole;

public static class Scenario1
{
    public static async Task RunAsync(ServiceProvider serviceProvider)
    {
        string folderPath = serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("FolderPath")!;
        var csvExtractor = serviceProvider.GetRequiredService<ICsvExtractor>();
        if (!Path.IsPathRooted(folderPath)) folderPath = Path.Combine(Constants.FromConsolePath, folderPath);
        await csvExtractor.ProcessCsvExtractionAsync(folderPath, new CancellationToken());
    }
}
