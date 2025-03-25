using PowerTradeCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PowerTradeConsole;

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

while (true)
{
    logger.LogInformation("Displaying menu to user.");
    MenuScreener.Run();
    var choice = Console.ReadLine();
    await (choice switch
    {
        "1" => Scenario1.RunAsync(serviceProvider),
        "2" => Scenario2.RunAsync(serviceProvider),
        "3" => Scenario3.RunAsync(serviceProvider),
        "4" => Scenario4.RunAsync(serviceProvider),
        "5" => Task.Run(() =>
        {
            logger.LogInformation("Exiting application...");
            Environment.Exit(0);
        }),
        _ => Task.Run(() =>
        {
            logger.LogWarning("Invalid choice. User input was invalid.");
            Console.WriteLine("Invalid choice. Please try again.");
        })
    });
    Console.WriteLine("Press any key to get back to the menu...");
    Console.ReadLine();
    Console.Clear();
}
