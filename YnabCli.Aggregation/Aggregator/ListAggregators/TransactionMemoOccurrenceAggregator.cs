using Ynab;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Extensions;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public class TransactionMemoOccurrenceAggregator(IEnumerable<Transaction> transactions)
    : ListAggregator<TransactionMemoOccurrenceAggregate>(transactions)
{
    protected override IEnumerable<TransactionMemoOccurrenceAggregate> ListAggregate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence()
            .AggregateMemoOccurrences();
}