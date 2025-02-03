using ConsoleTables;
using YnabProgress.ViewModels;

namespace YnabProgressConsole.Compilers;

public static class ConsoleTableCompiler
{
    public static ConsoleTable Compile(ViewModel viewModel)
    {
        var table = new ConsoleTable(viewModel.Columns.ToArray());
       
        foreach (var row in viewModel.Rows)
            table.AddRow(row.ToArray());
        
        return table;
    }
}