namespace YnabProgressConsole.Compilation.Aggregates;

public record TransactionMonthFlaggedAmountAggregate(
    string Flag,
    decimal CurrentAmount,
    int PercentageChange);