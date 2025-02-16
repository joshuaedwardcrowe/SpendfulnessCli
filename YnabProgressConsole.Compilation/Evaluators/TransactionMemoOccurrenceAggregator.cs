using Ynab;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Extensions;

namespace YnabProgressConsole.Compilation.Evaluators;

public class TransactionMemoOccurrenceAggregator(IEnumerable<Transaction> transactions)
    : Aggregator<IEnumerable<TransactionMemoOccurrenceAggregate>>(transactions)
{
    public override IEnumerable<TransactionMemoOccurrenceAggregate> Evaluate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence()
            .AggregateMemoOccurrences();
}