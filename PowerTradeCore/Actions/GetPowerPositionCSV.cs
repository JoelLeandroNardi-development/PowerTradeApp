using Axpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class GetPowerPositionCsv
{
    public static async Task<IResult> GenerateCSVAsync([FromServices] IPowerService powerService)
    {
        var data = await PowerPositionService.GetAggregatedPositionsAsync(powerService, DateTime.Now);
        var csv = CsvGenerator.GenerateCsvContent(data);
        var csvBytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return Results.File(csvBytes, "text/csv", CsvGenerator.GenerateCsvFileName(DateTime.UtcNow));
    }
}
