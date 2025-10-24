namespace Cli.Spendfulness.Aggregation.Aggregates;

public record TransactionMonthTotalAggregate(string Month, decimal TotalAmount, int PercentageChange);