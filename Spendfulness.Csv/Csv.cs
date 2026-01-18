namespace Spendfulness.Csv;

public class Csv
{
    public List<CsvRow> Rows { get; set; } = [];

    public Csv(List<CsvRow> rows)
    {
        Rows = rows;
    }

    public Task Write(string filePath)
    {
        var writer = new CsvWriter(this);
        return writer.Write(filePath);
    }
}