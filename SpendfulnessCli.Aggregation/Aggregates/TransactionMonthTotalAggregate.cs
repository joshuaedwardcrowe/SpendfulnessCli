namespace SpendfulnessCli.Aggregation.Aggregates;

public record TransactionMonthTotalAggregate(
    string Month,
    decimal TotalAmount,
    int PercentageChange);