namespace PowerTradeCore;

public interface IPowerPositionService
{
    Task<IEnumerable<AccumulatedPowerTrade>> GetAggregatedPositionsAsync(DateTime startDate);
}