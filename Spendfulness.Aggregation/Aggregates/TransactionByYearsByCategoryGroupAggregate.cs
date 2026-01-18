namespace Spendfulness.Aggregation.Aggregates;

public record TransactionByYearsByCategoryGroupAggregate(
    string CategoryGroupName,
    IEnumerable<TransactionByYearsByCategoryAggregate> CategoryAggregates)
{
    public decimal TotalAmountForYear(int year)
        => CategoryAggregates
            .Sum(ca => ca.TransactionsForYear(year).TotalAmount);
}