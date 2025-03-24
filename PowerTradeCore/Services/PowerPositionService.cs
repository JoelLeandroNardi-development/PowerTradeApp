using Axpo;

namespace PowerTradeCore;

public class PowerPositionService(IPowerService powerService) : IPowerPositionService
{
    public async Task<IEnumerable<AccumulatedPowerTrade>> GetAggregatedPositionsAsync(DateTime startDate)
    {
        DateTime tradeDate = startDate.AddDays(1);

        var trades = await powerService.GetTradesAsync(tradeDate);
        var hourlyVolumes = new double[24];

        foreach (var trade in trades)
        {
            for (var i = 0; i < 24; i++) hourlyVolumes[i] += trade.Periods.Where(x => x.Period == (i + 1)).SingleOrDefault().Volume;
        }

        return hourlyVolumes.Select((volume, hour) => new AccumulatedPowerTrade(startDate.AddHours(hour).ToUniversalTime().ToString(Constants.Iso8601Format), Math.Round(volume, 2)));
    }
}
