using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregator.ListAggregators;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.MonthlySpendingCreep;

public class MonthlySpendingCreepCommandHandler: CommandHandler, ICommandHandler<MonthlySpendingCreepCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly TransactionMonthChangeViewModelBuilder _transactionMonthChangeViewModelBuilder;

    public MonthlySpendingCreepCommandHandler(ConfiguredBudgetClient configuredBudgetClient, TransactionMonthChangeViewModelBuilder transactionMonthChangeViewModelBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _transactionMonthChangeViewModelBuilder = transactionMonthChangeViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(MonthlySpendingCreepCommand request, CancellationToken cancellationToken)
    {
        var budget = await _configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthTotalAggregator(transactions)
            .BeforeAggregation(o => o.FilterToSpending())
            .BeforeAggregation(o => o.FilterToOutflow());

        var viewModel = _transactionMonthChangeViewModelBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}