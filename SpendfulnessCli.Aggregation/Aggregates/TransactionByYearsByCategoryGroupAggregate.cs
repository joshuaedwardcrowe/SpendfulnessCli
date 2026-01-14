namespace SpendfulnessCli.Aggregation.Aggregates;

public class TransactionByYearsByCategoryGroupAggregate
{
    public string CategoryGroupName { get; set; }
    public IEnumerable<TransactionByYearsByCategoryAggregate> Aggregates { get; set; }
}