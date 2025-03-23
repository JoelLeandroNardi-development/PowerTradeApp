using Axpo;
using Microsoft.AspNetCore.Mvc;

namespace PowerTradeCore;

public static class GetPowerPositionCSV
{
    public static async Task GenerateCSVAsync([FromServices] IPowerService powerService, string folderPath)
    {
        var data = await PowerPositionService.GetAggregatedPositionsAsync(powerService, DateTime.Now);

        throw new NotImplementedException();
    }
}
