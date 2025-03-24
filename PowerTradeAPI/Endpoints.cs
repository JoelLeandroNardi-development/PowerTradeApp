using PowerTradeCore;

namespace PowerTradeAPI;

public static class Endpoints
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/powerposition/csv", GetPowerPositionCsv.GenerateCSVAsync)
            .WithName("GetCSV")
            .WithOpenApi();

        app.MapPost("/scheduleCsvDownload", CsvDownloadTaskScheduler.ScheduleTask)
            .WithName("TaskScheduler")
            .WithOpenApi();
    }
}
