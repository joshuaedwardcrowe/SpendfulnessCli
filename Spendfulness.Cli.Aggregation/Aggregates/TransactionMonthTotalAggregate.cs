namespace Spendfulness.Cli.Aggregation.Aggregates;

public record TransactionMonthTotalAggregate(string Month, decimal TotalAmount, int PercentageChange);