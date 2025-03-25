using PowerTradeCore;

namespace PowerTradeTests;

public class TestDataProvider
{
    public static string Hour1 = "2025-03-25T13:50:12.422Z";
    public static string Hour2 = "2025-03-25T14:50:12.422Z";

    public static List<AccumulatedPowerTrade> TestData = [
        new(Hour1, 100.5),
        new(Hour2, 200.75)
    ];
}
