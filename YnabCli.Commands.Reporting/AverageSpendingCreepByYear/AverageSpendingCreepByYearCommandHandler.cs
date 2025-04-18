using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregator.ListAggregators;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.AverageSpendingCreepByYear;

public class AverageSpendingCreepByYearCommandHandler : CommandHandler, ICommandHandler<AverageSpendingCreepByYearCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly TransactionYearAverageViewModelBuilder _transactionYearAverageViewModelBuilder;

    public AverageSpendingCreepByYearCommandHandler(ConfiguredBudgetClient configuredBudgetClient, TransactionYearAverageViewModelBuilder transactionYearAverageViewModelBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _transactionYearAverageViewModelBuilder = transactionYearAverageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(AverageSpendingCreepByYearCommand request, CancellationToken cancellationToken)
    {
        var budget =  await _configuredBudgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionAverageAcrossYearAggregator(transactions)
            .BeforeAggregation(y => y.FilterToSpending())
            .BeforeAggregation(y => y.FilterToOutflow());
        
        var viewModel = _transactionYearAverageViewModelBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}