namespace Cli.Spendfulness.Aggregation.Aggregates;

// TODO: I hate that this aggregate has a dictionary in it.

public record CategoryYearAverageAggregate(string CategoryName, Dictionary<string, decimal> AverageAmountByYears);