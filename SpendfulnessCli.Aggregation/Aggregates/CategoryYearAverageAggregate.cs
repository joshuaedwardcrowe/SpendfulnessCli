namespace SpendfulnessCli.Aggregation.Aggregates;

// TODO: Refactor - This aggregate represents more than a single data point. 

public record CategoryYearAverageAggregate(string CategoryName, Dictionary<int, decimal> AverageAmountByYears);