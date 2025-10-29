using Ynab;
using Ynab.Extensions;
using SpendfulnessCli.Aggregation.Extensions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

public class TransactionPayeeMemoOccurrenceYnabAggregator(IEnumerable<Transaction> transactions)
    : ListYnabAggregator<TransactionPayeeMemoOccurrenceAggregate>(transactions)
{
    protected override IEnumerable<TransactionPayeeMemoOccurrenceAggregate> ListAggregate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByPayeeMemoOccurence()
            .AggregatePayeeMemoOccurrences();
}