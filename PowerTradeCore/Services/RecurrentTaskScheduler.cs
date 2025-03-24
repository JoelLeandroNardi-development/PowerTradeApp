using System.Text.Json;

namespace PowerTradeCore;

public static class RecurrentTaskScheduler
{
    private static string _filePath => "appsettings.json";
    public static async Task Schedule(int? intervalMinutes, string? folderPath)
    {
        var json = await File.ReadAllTextAsync(_filePath);
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

        var updatedSettings = new Dictionary<string, object>
        {
            ["IntervalMinutes"] = intervalMinutes ?? jsonObject.GetProperty("IntervalMinutes").GetInt32(),
            ["FolderPath"] = folderPath ?? jsonObject.GetProperty("FolderPath").GetString()!
        };

        var updatedJson = JsonSerializer.Serialize(updatedSettings, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, updatedJson);
    }
}
