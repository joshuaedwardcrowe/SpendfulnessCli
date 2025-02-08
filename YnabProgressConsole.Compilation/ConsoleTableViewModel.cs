namespace YnabProgressConsole.Compilation;

public class ConsoleTableViewModel
{
    public List<string> Columns { get; set; } = [];
    public List<List<object>> Rows { get; set; } = [];
}