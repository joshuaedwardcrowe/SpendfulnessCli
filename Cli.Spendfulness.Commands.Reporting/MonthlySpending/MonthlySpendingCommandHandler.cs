using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregates;
using Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCommandHandler: CommandHandler, ICommandHandler<MonthlySpendingCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly TransactionMonthChangeCliTableBuilder _transactionMonthChangeCliTableBuilder;

    public MonthlySpendingCommandHandler(ConfiguredBudgetClient configuredBudgetClient, TransactionMonthChangeCliTableBuilder transactionMonthChangeCliTableBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _transactionMonthChangeCliTableBuilder = transactionMonthChangeCliTableBuilder;
    }

    public async Task<CliCommandOutcome> Handle(MonthlySpendingCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _transactionMonthChangeCliTableBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }

    private async Task<ListYnabAggregator<TransactionMonthTotalAggregate>> PrepareAggregator(MonthlySpendingCommand command)
    {
        var budget = await _configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthTotalYnabAggregator(transactions);

        if (command.CategoryId.HasValue)
        {
            aggregator.BeforeAggregation(o => o.FilterToCategories(command.CategoryId.Value));
        }
        
        return aggregator;
    }
}