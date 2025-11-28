using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;

namespace SpendfulnessCli.Commands.Reporting.SplittableTransactions;

public class SplittableTransactionsCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : ICliCommandHandler<SplittableTransactionsCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SplittableTransactionsCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new PotentialTransactionSplitListAggregator(transactions);
        
        var aggregatorOutcome = new ListAggregatorCliCommandOutcome<PotentialTransactionSplitAggregate>(aggregator);
        
        var table = new TransactionPotentialSplitCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        var tableOutcome = new CliCommandTableOutcome(table);
        
        return
        [
            aggregatorOutcome,
            tableOutcome,
        ];
    }
}