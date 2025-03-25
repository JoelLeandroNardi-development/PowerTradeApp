using System.Text.Json.Serialization;

namespace PowerTradeCore;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(TaskSchedulerInput))]
public partial class AppSettingsJsonContext : JsonSerializerContext
{
}