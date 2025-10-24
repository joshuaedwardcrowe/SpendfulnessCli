namespace Cli.Spendfulness.Aggregation.Aggregates;

public record TransactionYearAverageAggregate(string Year, decimal AverageAmount, int PercentageChange);