using Spendfulness.Cli.Aggregation.Aggregates;
using Ynab.Collections;

namespace Spendfulness.Cli.Aggregation.Extensions;

public static class TransactionByMemoOccurrenceByPayeeNameExtensions
{
    public static IEnumerable<TransactionPayeeMemoOccurrenceAggregate> AggregatePayeeMemoOccurrences(
        this IEnumerable<TransactionsByMemoOccurrenceByPayeeName> transactionGroups)
    {
        foreach (var transactionGroup in transactionGroups)
        {
            foreach (var transactionSubGroup in transactionGroup.TransactionsByMemoOccurrences)
            {
                var averageAmount = transactionSubGroup.Transactions.Average(o => o.Amount);
                var totalAmount = transactionSubGroup.Transactions.Sum(o => o.Amount);
                
                var occurrence = new TransactionPayeeMemoOccurrenceAggregate(
                    transactionGroup.PayeeName,
                    transactionSubGroup.Memo,
                    transactionSubGroup.MemoOccurence,
                    averageAmount,
                    totalAmount);
                
                yield return occurrence;
            }
        }
    }
}