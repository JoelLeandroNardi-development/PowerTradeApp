using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace PowerTradeCore;

public static class CsvGenerator
{
    public static string GenerateCsvFileName(DateTime dateTime)
    {
        var dayAhead = dateTime.AddDays(1).ToString("yyyyMMdd");
        var extractionTimestamp = dateTime.ToString("yyyyMMddHHmm");
        return $"PowerPosition_{dayAhead}_{extractionTimestamp}.csv";
    }

    public static string GenerateCsvContent(IEnumerable<AccumulatedPowerTrade> data)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        using var stringWriter = new StringWriter();
        using var csv = new CsvWriter(stringWriter, config);
        csv.WriteHeader<AccumulatedPowerTrade>();
        csv.NextRecord();
        csv.WriteRecords(data);
        csv.Flush();

        return stringWriter.ToString();
    }
}
