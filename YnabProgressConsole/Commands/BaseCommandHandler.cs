using ConsoleTables;
using Ynab.Clients;
using YnabProgress.ViewModels;

namespace YnabProgressConsole.Commands;

public abstract class BaseCommandHandler
{
    protected readonly BudgetsClient _budgetsClient;

    protected BaseCommandHandler(BudgetsClient budgetsClient)
    {
        _budgetsClient = budgetsClient;
    }
    
    protected ConsoleTable Compile(ViewModel viewModel)
    {
        var table = new ConsoleTable(viewModel.Columns.ToArray());
       
        foreach (var row in viewModel.Rows)
            table.AddRow(row.ToArray());
        
        return table;
    }
}