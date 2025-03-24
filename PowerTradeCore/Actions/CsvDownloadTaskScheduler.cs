using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class CsvDownloadTaskScheduler
{
    public static async Task<IResult> ScheduleTask([FromServices] IRecurrentTaskScheduler recurrentTaskScheduler, [FromBody] TaskSchedulerInput input)
    {
        await recurrentTaskScheduler.Schedule(input.IntervalMinutes, input.FolderPath);
        return Results.Ok("Settings updated successfully!");
    }
}
