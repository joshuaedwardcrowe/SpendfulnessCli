using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

// TODO: Write unit tests.
public class MonthlySpendingCliCommandHandler: CliCommandHandler, ICliCommandHandler<MonthlySpendingCliCommand>
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;

    public MonthlySpendingCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
    }

    public async Task<CliCommandOutcome[]> Handle(MonthlySpendingCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(cliCommand);
        
        var table = new TransactionMonthChangeCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return [
            new CliCommandTableOutcome(table),
            new CliCommandAggregatorOutcome<IEnumerable<TransactionMonthTotalAggregate>>(aggregator)
        ];
    }

    private async Task<ListYnabAggregator<TransactionMonthTotalAggregate>> PrepareAggregator(MonthlySpendingCliCommand cliCommand)
    {
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthTotalYnabAggregator(transactions);

        if (cliCommand.CategoryId.HasValue)
        {
            aggregator.BeforeAggregation(o => o.FilterToCategories(cliCommand.CategoryId.Value));
        }
        
        return aggregator;
    }
}