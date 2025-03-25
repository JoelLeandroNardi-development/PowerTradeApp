using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class GetPowerPositionCsv
{
    public static async Task<IResult> GenerateCSVAsync([FromServices] IPowerPositionService powerPositionService, [FromServices] ICsvGenerator csvGenerator)
    {
        var now = DateTime.Now;
        var data = await powerPositionService.GetAggregatedPositionsAsync(now);
        var csv = csvGenerator.GenerateCsvContent(data);
        var csvBytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return Results.File(csvBytes, "text/csv", csvGenerator.GenerateCsvFileName(now));
    }
}
