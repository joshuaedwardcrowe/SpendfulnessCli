using ConsoleTables;
using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Commands;

public abstract class CommandHandler
{
    protected ConsoleTable Compile(ViewModel viewModel)
    {
        var table = new ConsoleTable(viewModel.Columns.ToArray());
       
        foreach (var row in viewModel.Rows)
            table.AddRow(row.ToArray());
        
        return table;
    }
}