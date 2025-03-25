using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace PowerTradeCore;

public class RecurrentTaskScheduler(ILogger<RecurrentTaskScheduler> logger) : IRecurrentTaskScheduler
{
    public async Task Schedule(int? intervalMinutes, string? folderPath) => 
        await ChangeSettings(intervalMinutes, folderPath, Constants.FilePath);

    public async Task ScheduleFromConsole(int? intervalMinutes, string? folderPath) =>
        await ChangeSettings(intervalMinutes, folderPath, Constants.FromConsolePath + Constants.FilePath);

    private async Task ChangeSettings(int? intervalMinutes, string? folderPath, string path)
    {
        logger.LogInformation("Changing settings.");
        var json = await File.ReadAllTextAsync(path);
        var jsonObject = JsonSerializer.Deserialize(json, AppSettingsJsonContext.Default.TaskSchedulerInput);
        var settings = new TaskSchedulerInput(
            intervalMinutes ?? jsonObject!.IntervalMinutes,
            folderPath ?? jsonObject!.FolderPath);
        var updatedJson = JsonSerializer.Serialize(settings, AppSettingsJsonContext.Default.TaskSchedulerInput);
        await File.WriteAllTextAsync(path, updatedJson);
        logger.LogInformation("Settings changed.");
    }
}
