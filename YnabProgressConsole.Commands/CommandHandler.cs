using ConsoleTables;
using YnabProgressConsole.Compilation;

namespace YnabProgressConsole.Commands;

public abstract class CommandHandler
{
    protected ConsoleTable Compile(ConsoleTableViewModel tableViewModel)
    {
        var table = new ConsoleTable(tableViewModel.Columns.ToArray());
       
        foreach (var row in tableViewModel.Rows)
            table.AddRow(row.ToArray());
        
        return table;
    }
}