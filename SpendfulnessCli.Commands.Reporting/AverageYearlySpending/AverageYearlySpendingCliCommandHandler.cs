using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using Spendfulness.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using YnabSharp.Extensions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<AverageYearlySpendingCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(AverageYearlySpendingCliCommand request, CancellationToken cancellationToken)
    {
        var budget =  await spendfulnessBudgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionAverageAcrossYearYnabListAggregator(transactions)
            .BeforeAggregation(y => y.FilterToSpending())
            .BeforeAggregation(y => y.FilterToOutflow());
        
        var viewModel = new TransactionYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return OutcomeAs(viewModel);
    }
}