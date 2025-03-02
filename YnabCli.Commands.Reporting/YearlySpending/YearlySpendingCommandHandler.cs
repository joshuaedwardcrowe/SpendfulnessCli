using ConsoleTables;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.YearlySpending;

public class YearlySpendingCommandHandler : CommandHandler, ICommandHandler<YearlySpendingCommand>
{
    private readonly DbBudgetClient _budgetClient;
    private readonly CategoryYearAverageViewModelBuilder _viewModelBuilder;

    public YearlySpendingCommandHandler(DbBudgetClient budgetClient, CategoryYearAverageViewModelBuilder viewModelBuilder)
    {
        _budgetClient = budgetClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlySpendingCommand request, CancellationToken cancellationToken)
    {
        var budget =  await _budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new CategoryYearAverageAggregator(transactions);

        var viewModel = _viewModelBuilder
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}