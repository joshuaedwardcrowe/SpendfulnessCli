namespace SpendfulnessCli.Aggregation.Aggregates;

public record TransactionYearAverageAggregate(int Year, decimal AverageAmount, int PercentageChange);