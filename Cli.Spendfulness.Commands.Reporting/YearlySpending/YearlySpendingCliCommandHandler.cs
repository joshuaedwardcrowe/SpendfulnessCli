using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregator;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.YearlySpending;

public class YearlySpendingCliCommandHandler(SpendfulnessBudgetClient budgetClient)
    : CliCommandHandler, ICliCommandHandler<YearlySpendingCliCommand>
{
    public async Task<CliCommandOutcome> Handle(YearlySpendingCliCommand request, CancellationToken cancellationToken)
    {
        var budget =  await budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new CategoryYearAverageYnabAggregator(transactions)
            .BeforeAggregation(t => t.FilterOutTransfers())
            .BeforeAggregation(t => t.FilterOutAutomations());

        var viewModel = new CategoryYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}