using Ynab.Collections;

namespace SpendfulnessCli.Aggregation.Aggregates;

public class TransactionByYearsByCategoryAggregate
{
    public string CategoryName { get; set; }
    public IEnumerable<SplitTransactionsByYear> TransactionsByYears { get; set; }
}