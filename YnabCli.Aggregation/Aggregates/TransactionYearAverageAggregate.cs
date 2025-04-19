namespace YnabCli.Aggregation.Aggregates;

public record TransactionYearAverageAggregate(string Year, decimal AverageAmount, int PercentageChange);