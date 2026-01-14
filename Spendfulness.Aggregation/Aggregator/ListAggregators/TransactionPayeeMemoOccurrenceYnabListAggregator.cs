using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Aggregation.Extensions;
using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;
using Ynab.Extensions;

namespace Spendfulness.Aggregation.Aggregator.ListAggregators;

public class TransactionPayeeMemoOccurrenceYnabListAggregator(IEnumerable<Transaction> transactions)
    : YnabListAggregator<TransactionPayeeMemoOccurrenceAggregate>(transactions)
{
    protected override IEnumerable<TransactionPayeeMemoOccurrenceAggregate> GenerateAggregate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByPayeeMemoOccurence()
            .AggregatePayeeMemoOccurrences();
}