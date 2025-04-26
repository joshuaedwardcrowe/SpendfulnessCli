using Ynab;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Extensions;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public class TransactionPayeeMemoOccurrenceAggregator(IEnumerable<Transaction> transactions)
    : ListAggregator<TransactionPayeeMemoOccurrenceAggregate>(transactions)
{
    protected override IEnumerable<TransactionPayeeMemoOccurrenceAggregate> ListAggregate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByPayeeMemoOccurence()
            .AggregatePayeeMemoOccurrences();
}