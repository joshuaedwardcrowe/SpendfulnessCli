namespace SpendfulnessCli.Aggregation.Aggregates;

public record TransactionPayeeMemoOccurrenceAggregate(
    string PayeeName,
    string? Memo,
    int MemoOccurrence,
    decimal AverageAmount,
    decimal TotalAmount);