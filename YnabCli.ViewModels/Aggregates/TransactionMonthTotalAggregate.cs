namespace YnabCli.ViewModels.Aggregates;

public record TransactionMonthTotalAggregate(string Month, decimal TotalAmount, int PercentageChange);