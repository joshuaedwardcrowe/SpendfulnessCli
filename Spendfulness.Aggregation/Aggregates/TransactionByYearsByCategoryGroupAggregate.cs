namespace Spendfulness.Aggregation.Aggregates;

public class TransactionByYearsByCategoryGroupAggregate
{
    public string CategoryGroupName { get; set; }
    public IEnumerable<TransactionByYearsByCategoryAggregate> CategoryAggregates { get; set; }
}