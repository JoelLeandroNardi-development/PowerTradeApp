using PowerTradeCore;

namespace PowerTradeTests;

public class CsvGeneratorTests
{
    private readonly CsvGenerator _subject;
    public CsvGeneratorTests() => _subject = new CsvGenerator();

    [Fact]
    public void GenerateCsvFileName_ShouldReturnCorrectFileName()
    {
        // Arrange
        var testDate = new DateTime(2025, 3, 25, 14, 30, 0, DateTimeKind.Utc);

        // Act
        var fileName = _subject.GenerateCsvFileName(testDate);

        // Assert
        Assert.Equal("PowerPosition_20250326_202503251430.csv", fileName);
    }

    [Fact]
    public void GenerateCsvContent_ShouldGenerateValidCsv()
    {
        // Arrange
        DateTime now = DateTime.UtcNow;
        var hour1 = now.AddHours(1).ToString();
        var hour2 = now.AddHours(2).ToString();
        var data = new List<AccumulatedPowerTrade>
        {
            new(hour1, 100.5),
            new(hour2, 200.75)
        };

        // Act
        var csvContent = _subject.GenerateCsvContent(data);

        // Assert
        Assert.NotNull(csvContent);
        Assert.Contains("Datetime;Volume", csvContent);
        Assert.Contains($"{hour1};100.5", csvContent);
        Assert.Contains($"{hour2};200.75", csvContent);
    }
}
