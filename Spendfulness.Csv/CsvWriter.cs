using System.Globalization;

namespace Spendfulness.Csv;

public class CsvWriter
{
    public Csv Csv { get; set; }
    
    public CsvWriter(Csv csv)
    {
        Csv = csv;
    }

    public async Task Write(string filePath)
    {
        await using var streamWriter = new StreamWriter(filePath);
        await using var csvWriter = new CsvHelper.CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        
        foreach (var csvRow in Csv.Rows)
        {
            foreach (var csvColumn in csvRow.Columns)
            {
                csvWriter.WriteField(csvColumn);
            }
            
            await csvWriter.NextRecordAsync();
        }
    }
}