using Ynab;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Extensions;

namespace YnabProgressConsole.Compilation.Evaluators;

public class TransactionMemoOccurrenceEvaluator(IEnumerable<Transaction> transactions)
    : YnabEvaluator<IEnumerable<TransactionMemoOccurrenceAggregate>>(transactions)
{
    public override IEnumerable<TransactionMemoOccurrenceAggregate> Evaluate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence()
            .AggregateMemoOccurrences();
}