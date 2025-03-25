using Microsoft.Extensions.DependencyInjection;
using PowerTradeCore;

namespace PowerTradeConsole;

public static class Scenario4
{
    public static async Task RunAsync(ServiceProvider serviceProvider)
    {
        var iRecurrentTaskScheduler = serviceProvider.GetRequiredService<IRecurrentTaskScheduler>();
        Console.WriteLine("Enter the new desired interval (in minutes):");
        int? newInterval = int.Parse(Console.ReadLine()!);
        await iRecurrentTaskScheduler.ScheduleFromConsole(newInterval, null);
    }
}
