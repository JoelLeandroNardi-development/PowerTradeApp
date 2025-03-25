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
        var data = TestDataProvider.TestData;

        // Act
        var csvContent = _subject.GenerateCsvContent(data);

        // Assert
        Assert.NotNull(csvContent);
        Assert.Contains("Datetime;Volume", csvContent);
        Assert.Contains($"{TestDataProvider.Hour1};100.5", csvContent);
        Assert.Contains($"{TestDataProvider.Hour2};200.75", csvContent);
    }
}
