namespace YnabCli.Aggregation.Aggregates;

public record CategoryYearAverageAggregate(string CategoryName, Dictionary<string, decimal> AverageAmountByYears);