using Ynab.Collections;

namespace Ynab.Extensions;

public static class TransactionsByPayeeNameExtensions
{
    public static IEnumerable<TransactionsByMemoOccurrenceByPayeeName> GroupByMemoOccurence(
        this IEnumerable<TransactionsByPayeeName> transactionsByPayeeNames, int? minimumOccurences = 0)
    {
        foreach (var transactionsByPayeeName in transactionsByPayeeNames)
        {
            var memoOccurences = transactionsByPayeeName
                .Transactions
                .GroupBy(t => t.Memo)
                .Select(grouping => new TransactionsByMemoOccurence
                {
                    Memo = grouping.Key,
                    MemoOccurence = grouping.Count(),
                    Transactions = grouping.ToList()
                });

            if (minimumOccurences is not null)
            {
                memoOccurences = memoOccurences
                    .Where(x => x.MemoOccurence >= minimumOccurences);
            }
            
            memoOccurences = memoOccurences
                .OrderByDescending(t => t.MemoOccurence);

            yield return new TransactionsByMemoOccurrenceByPayeeName
            {
                PayeeName = transactionsByPayeeName.PayeeName,
                TransactionsByMemoOccurences = memoOccurences
            };
        }
    }
}