using Cli.Spendfulness.Aggregation.Aggregates;

namespace Cli.Spendfulness.Aggregation.Extensions;

public static class TransactionMemoOccurrenceAggregateExtensions
{
    public static IEnumerable<TransactionPayeeMemoOccurrenceAggregate> FilterToPayeeName(
        this IEnumerable<TransactionPayeeMemoOccurrenceAggregate> source, string payeeName) 
            => source.Where(agg => agg.PayeeName == payeeName);
    
    public static IEnumerable<TransactionPayeeMemoOccurrenceAggregate> FilterToMinimumOccurrences(
        this IEnumerable<TransactionPayeeMemoOccurrenceAggregate> source, int minimumOccurrences)
            => source.Where(agg => agg.MemoOccurrence >= minimumOccurrences);
}