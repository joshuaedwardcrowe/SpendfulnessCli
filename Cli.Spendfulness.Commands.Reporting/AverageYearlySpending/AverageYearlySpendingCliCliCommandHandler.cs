using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Ynab.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<AverageYearlySpendingCliCommand>
{
    public async Task<CliCommandOutcome> Handle(AverageYearlySpendingCliCommand request, CancellationToken cancellationToken)
    {
        var budget =  await spendfulnessBudgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionAverageAcrossYearYnabAggregator(transactions)
            .BeforeAggregation(y => y.FilterToSpending())
            .BeforeAggregation(y => y.FilterToOutflow());
        
        var viewModel = new TransactionYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }
}