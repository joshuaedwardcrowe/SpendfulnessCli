using Ynab;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Extensions;

namespace YnabProgressConsole.Compilation.Evaluators;

public class TransactionMemoOccurrenceEvaluator : YnabEvaluator<IEnumerable<TransactionMemoOccurrenceAggregate>>
{
    public TransactionMemoOccurrenceEvaluator(
        IEnumerable<Account>? accounts = null,
        IEnumerable<CategoryGroup>? categoryGroups = null,
        IEnumerable<Category>? categories = null,
        IEnumerable<ScheduledTransaction>? scheduledTransactions = null,
        IEnumerable<Transaction>? transactions = null)
        : base(
            accounts,
            categoryGroups,
            categories,
            scheduledTransactions,
            transactions)
    {}

    public override IEnumerable<TransactionMemoOccurrenceAggregate> Evaluate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence()
            .Aggregate();
}