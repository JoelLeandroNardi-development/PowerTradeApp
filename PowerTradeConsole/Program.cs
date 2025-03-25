using PowerTradeCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.GetFullPath("../../../../PowerTradeAPI"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddCoreServices(true);
var serviceProvider = services.BuildServiceProvider();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Console app started.");
var csvExtractor = serviceProvider.GetRequiredService<ICsvExtractor>();
var iRecurrentTaskScheduler = serviceProvider.GetRequiredService<IRecurrentTaskScheduler>();

while (true)
{
    logger.LogInformation("Displaying menu to user.");
    Console.WriteLine("Welcome to the Power Position CSV Downloader.");
    Console.WriteLine("============================================================");
    Console.WriteLine("Select an option:");
    Console.WriteLine("1 - Download CSV to the default folder (set in configuration file)");
    Console.WriteLine("2 - Download CSV to a custom folder (this option also changes configuration file");
    Console.WriteLine("3 - Change default folder path");
    Console.WriteLine("4 - Change interval of periodical download of CSV");
    Console.WriteLine("5 - Exit");
    Console.WriteLine("============================================================");

    var choice = Console.ReadLine();
    CancellationToken stoppingToken = new CancellationToken();
    string folderPath = string.Empty;
    switch (choice)
    {
        case "1":
            folderPath = serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("FolderPath")!;
            if (!Path.IsPathRooted(folderPath)) folderPath = Path.Combine(Constants.FromConsolePath, folderPath);
            await csvExtractor.ProcessCsvExtractionAsync(folderPath, stoppingToken);
            break;

        case "2":
            Console.WriteLine("Enter the folder path where you want to save the CSV:");
            folderPath = Console.ReadLine()!;
            if (!Path.IsPathRooted(folderPath)) folderPath = Path.Combine(Constants.FromConsolePath, folderPath);
            await csvExtractor.ProcessCsvExtractionAsync(folderPath, stoppingToken);
            break;

        case "3":
            Console.WriteLine("Enter the new default folder path:");
            string? newPath = Console.ReadLine();
            await iRecurrentTaskScheduler.ScheduleFromConsole(null, newPath);
            break;

        case "4":
            Console.WriteLine("Enter the new desired interval (in minutes):");
            int? newInterval = int.Parse(Console.ReadLine()!);
            await iRecurrentTaskScheduler.ScheduleFromConsole(newInterval, null);
            break;

        case "5":
            logger.LogInformation("Exiting application...");
            return;

        default:
            logger.LogWarning("Invalid choice. User input was invalid.");
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
    Console.WriteLine("Press any key to get back to the menu...");
    Console.ReadLine();
    Console.Clear();
}
