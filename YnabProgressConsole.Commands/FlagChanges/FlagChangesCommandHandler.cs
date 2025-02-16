using ConsoleTables;
using Ynab.Clients;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModelBuilders;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Commands.FlagChanges;

public class FlagChangesCommandHandler : CommandHandler, ICommandHandler<FlagChangesCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly TransactionMonthFlaggedViewModelBuilder _viewModelBuilder;

    public FlagChangesCommandHandler(
        BudgetsClient budgetsClient,
        TransactionMonthFlaggedViewModelBuilder viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(FlagChangesCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        var budget = budgets.First();
        
        var categoryGroups = await budget.GetCategoryGroups();
        var transactions = await budget.GetTransactions();
        
        var evaluator = new TransactionMonthFlaggedAggregator(categoryGroups, transactions);
        
        var viewModel = _viewModelBuilder
            .AddAggregator(evaluator)
            .AddColumnNames(TransactionMonthFlaggedViewModel.GetColumnNames())
            .Build();
        
        return Compile(viewModel);
    }
}