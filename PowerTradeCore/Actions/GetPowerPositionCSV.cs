using Axpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class GetPowerPositionCSV
{
    public static async Task<IResult> GenerateCSVAsync([FromServices] IPowerService powerService)
    {
        var csv = await PowerPositionService.GetAggregatedPositionsCsvAsync(powerService, DateTime.Now);
        var csvBytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return Results.File(csvBytes, "text/csv", CsvGenerator.GenerateCsvFileName(DateTime.Now));
    }
}
