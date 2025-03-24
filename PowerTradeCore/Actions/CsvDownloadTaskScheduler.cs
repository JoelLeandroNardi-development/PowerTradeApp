using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class CsvDownloadTaskScheduler
{
    public static async Task<IResult> ScheduleTask([FromBody] TaskSchedulerInput input)
    {
        await RecurrentTaskScheduler.Schedule(input.IntervalMinutes, input.FolderPath);
        return Results.Ok("Settings updated successfully!");
    }
}
