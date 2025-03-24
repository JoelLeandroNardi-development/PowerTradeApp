using System.Text.Json;

namespace PowerTradeCore;

public static class RecurrentTaskScheduler
{
    public static async Task Schedule(int? intervalMinutes, string? folderPath)
    {
        var json = await File.ReadAllTextAsync(Constants.FilePath);
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

        var updatedSettings = new Dictionary<string, object>
        {
            [Constants.IntervalMinutes] = intervalMinutes ?? jsonObject.GetProperty(Constants.IntervalMinutes).GetInt32(),
            [Constants.FolderPath] = folderPath ?? jsonObject.GetProperty(Constants.FolderPath).GetString()!
        };

        var updatedJson = JsonSerializer.Serialize(updatedSettings, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(Constants.FilePath, updatedJson);
    }
}
