using YnabCli.Aggregation.Aggregates;

namespace YnabCli.Aggregation.Extensions;

public static class TransactionMemoOccurrenceAggregateExtensions
{
    public static IEnumerable<TransactionMemoOccurrenceAggregate> FilterToPayeeName(
        this IEnumerable<TransactionMemoOccurrenceAggregate> source, string payeeName) 
            => source.Where(agg => agg.PayeeName == payeeName);
    
    public static IEnumerable<TransactionMemoOccurrenceAggregate> FilterToMinimumOccurrences(
        this IEnumerable<TransactionMemoOccurrenceAggregate> source, int minimumOccurrences)
            => source.Where(agg => agg.MemoOccurrence >= minimumOccurrences);
}