namespace Spendfulness.Aggregation.Aggregates;

public record TransactionMonthFlaggedAmountAggregate(
    string Flag,
    decimal CurrentAmount,
    int PercentageChange);