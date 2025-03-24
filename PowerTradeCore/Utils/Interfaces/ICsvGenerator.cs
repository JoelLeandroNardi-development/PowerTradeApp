namespace PowerTradeCore
{
    public interface ICsvGenerator
    {
        string GenerateCsvContent(IEnumerable<AccumulatedPowerTrade> data);
        string GenerateCsvFileName(DateTime dateTime);
    }
}