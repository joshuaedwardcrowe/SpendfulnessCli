using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCommandHandler: CommandHandler, ICommandHandler<MonthlySpendingCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly TransactionMonthChangeViewModelBuilder _transactionMonthChangeViewModelBuilder;

    public MonthlySpendingCommandHandler(ConfiguredBudgetClient configuredBudgetClient, TransactionMonthChangeViewModelBuilder transactionMonthChangeViewModelBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _transactionMonthChangeViewModelBuilder = transactionMonthChangeViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(MonthlySpendingCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _transactionMonthChangeViewModelBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }

    private async Task<ListAggregator<TransactionMonthTotalAggregate>> PrepareAggregator(MonthlySpendingCommand command)
    {
        var budget = await _configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthTotalAggregator(transactions);

        if (command.CategoryId.HasValue)
        {
            aggregator.BeforeAggregation(o => o.FilterToCategories(command.CategoryId.Value));
        }
        
        return aggregator;
    }
}