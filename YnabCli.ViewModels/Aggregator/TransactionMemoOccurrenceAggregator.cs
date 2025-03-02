using Ynab;
using Ynab.Extensions;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Extensions;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.Aggregator;

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