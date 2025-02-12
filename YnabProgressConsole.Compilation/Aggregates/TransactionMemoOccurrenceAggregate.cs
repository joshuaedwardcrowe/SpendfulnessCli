namespace YnabProgressConsole.Compilation.Aggregates;

public record TransactionMemoOccurrenceAggregate(
    string PayeeName,
    string Memo,
    int MemoOccurrence,
    decimal AverageAmount);