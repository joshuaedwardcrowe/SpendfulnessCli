namespace Spendfulness.Csv;

public class CsvRow
{
    public List<string> Columns { get; set; } = [];
    
    public CsvRow(List<string> columns)
    {
        Columns = columns;
    }
}