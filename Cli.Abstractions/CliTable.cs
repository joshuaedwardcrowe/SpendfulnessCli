namespace Cli.Abstractions;

public class CliTable
{
    public List<string> Columns { get; set; } = [];
    public List<List<object>> Rows { get; set; } = [];

    public bool ShowRowCount { get; set; } = true;
}