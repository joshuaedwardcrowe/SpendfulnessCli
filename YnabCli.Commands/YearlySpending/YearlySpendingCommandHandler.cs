using ConsoleTables;
using Ynab.Clients;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.YearlySpending;

public class YearlySpendingCommandHandler : CommandHandler, ICommandHandler<YearlySpendingCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly CategoryYearAverageViewModelBuilder _viewModelBuilder;
    
    public YearlySpendingCommandHandler(BudgetsClient budgetsClient, CategoryYearAverageViewModelBuilder viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlySpendingCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();

        var budget = budgets.First();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new CategoryYearAverageAggregator(transactions);

        var viewModel = _viewModelBuilder
            .AddAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}