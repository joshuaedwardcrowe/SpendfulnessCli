using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.MonthlySpending;

// TODO: Write unit tests.
public class MonthlySpendingCliCommandHandler: CliCommandHandler, ICliCommandHandler<MonthlySpendingCliCommand>
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;

    public MonthlySpendingCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
    }

    public async Task<CliCommandOutcome> Handle(MonthlySpendingCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(cliCommand);

        var viewModel = new TransactionMonthChangeCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
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