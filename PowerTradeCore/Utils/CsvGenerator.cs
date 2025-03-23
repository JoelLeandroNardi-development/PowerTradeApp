using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace PowerTradeCore;

public static class CsvGenerator
{
    private static string GenerateCsvFileName(DateTime dateTime)
    {
        var dayAhead = dateTime.AddDays(1).ToString("yyyyMMdd");
        var extractionTimestamp = dateTime.ToString("yyyyMMddHHmm");
        return $"PowerPosition_{dayAhead}_{extractionTimestamp}.csv";
    }

    private static void WriteCsv(string filePath, IEnumerable<AccumulatedPowerTrade> data)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, config);
        csv.WriteRecords(data);
    }
}
