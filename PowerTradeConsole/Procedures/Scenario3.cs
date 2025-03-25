using Microsoft.Extensions.DependencyInjection;
using PowerTradeCore;

namespace PowerTradeConsole;

public static class Scenario3
{
    public static async Task RunAsync(ServiceProvider serviceProvider)
    {
        var iRecurrentTaskScheduler = serviceProvider.GetRequiredService<IRecurrentTaskScheduler>();
        Console.WriteLine("Enter the new default folder path:");
        string? newPath = Console.ReadLine();
        await iRecurrentTaskScheduler.ScheduleFromConsole(null, newPath);
    }
}
