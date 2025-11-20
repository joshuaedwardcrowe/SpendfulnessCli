using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<AverageYearlySpendingCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(AverageYearlySpendingCliCommand request, CancellationToken cancellationToken)
    {
        var budget =  await spendfulnessBudgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionAverageAcrossYearYnabAggregator(transactions)
            .BeforeAggregation(y => y.FilterToSpending())
            .BeforeAggregation(y => y.FilterToOutflow());
        
        var viewModel = new TransactionYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return OutcomeAs(viewModel);
    }
}