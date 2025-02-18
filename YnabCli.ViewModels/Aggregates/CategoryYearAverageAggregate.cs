namespace YnabCli.ViewModels.Aggregates;

public record CategoryYearAverageAggregate(string CategoryName, Dictionary<string, decimal> AverageAmountByYears);