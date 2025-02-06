using System.Transactions;
using Ynab.Collections;

namespace Ynab.Extensions;

public static class TransactionsByPayeeNameExtensions
{
    public static IEnumerable<TransactionsByMemoOccurenceByPayeeName> GroupbyMemoOccurence(
        this IEnumerable<TransactionsByPayeeName> transactionsByPayeeNames)
    {
        foreach (var transactionsbyPayeeName in transactionsByPayeeNames)
        {
            var memoOccurences = transactionsbyPayeeName
                .Transactions
                .GroupBy(t => t.Memo)
                .Select(grouping => new TransactionsByMemoOccurence
                {
                    Memo = grouping.Key,
                    MemoOccurence = grouping.Count(),
                    Transactions = grouping.ToList()
                });

            yield return new TransactionsByMemoOccurenceByPayeeName
            {
                PayeeName = transactionsbyPayeeName.PayeeName,
                TransactionsByMemoOccurences = memoOccurences
            };
        }
    }
}