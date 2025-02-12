using YnabProgressConsole.Compilation.Aggregates;

namespace YnabProgressConsole.Compilation.Extensions;

public static class TransactionMemoOccurrenceAggregateExtensions
{
    public static IEnumerable<TransactionMemoOccurrenceAggregate> FilterByPayeeName(
        this IEnumerable<TransactionMemoOccurrenceAggregate> source, string payeeName) 
            => source.Where(agg => agg.PayeeName == payeeName);
    
    public static IEnumerable<TransactionMemoOccurrenceAggregate> FilterByMinimumOccurrences(
        this IEnumerable<TransactionMemoOccurrenceAggregate> source, int minimumOccurrences)
            => source.Where(agg => agg.MemoOccurrence >= minimumOccurrences);
}