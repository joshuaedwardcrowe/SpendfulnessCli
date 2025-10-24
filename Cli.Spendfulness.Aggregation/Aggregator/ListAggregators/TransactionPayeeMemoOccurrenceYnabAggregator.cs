using Cli.Spendfulness.Aggregation.Aggregates;
using Ynab;
using Ynab.Extensions;
using Cli.Spendfulness.Aggregation.Extensions;

namespace Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;

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