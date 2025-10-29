using Ynab;
using Ynab.Extensions;
using Spendfulness.Cli.Aggregation.Extensions;
using Spendfulness.Cli.Aggregation.Aggregates;

namespace Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;

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