using Ynab.Collections;

namespace Ynab.Extensions;

public static class TransactionsByPayeeNameExtensions
{
    public static IEnumerable<TransactionsByMemoOccurrenceByPayeeName> GroupByMemoOccurence(
        this IEnumerable<TransactionsByPayeeName> transactionsByPayeeNames)
    {
        foreach (var transactionsByPayeeName in transactionsByPayeeNames)
        {
            var memoOccurrenceGroups = transactionsByPayeeName
                .GroupByMemoOccurrence()
                .OrderByDescending(memoOccurrenceGroup => memoOccurrenceGroup.MemoOccurence);

            yield return new TransactionsByMemoOccurrenceByPayeeName
            {
                PayeeName = transactionsByPayeeName.PayeeName,
                TransactionsByMemoOccurences = memoOccurrenceGroups
            };
        }
    }
}