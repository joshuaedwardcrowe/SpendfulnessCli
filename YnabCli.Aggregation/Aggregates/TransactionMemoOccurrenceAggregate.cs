namespace YnabCli.Aggregation.Aggregates;

public record TransactionMemoOccurrenceAggregate(
    string PayeeName,
    string? Memo,
    int MemoOccurrence,
    decimal AverageAmount,
    decimal TotalAmount);