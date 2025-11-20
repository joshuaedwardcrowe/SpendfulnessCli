using Ynab;
using Ynab.Extensions;
using SpendfulnessCli.Aggregation.Extensions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

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