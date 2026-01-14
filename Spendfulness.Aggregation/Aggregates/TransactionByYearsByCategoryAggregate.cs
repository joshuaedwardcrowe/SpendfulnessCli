using Ynab.Collections;

namespace Spendfulness.Aggregation.Aggregates;

public class TransactionByYearsByCategoryAggregate
{
    public string CategoryName { get; set; }
    public IEnumerable<SplitTransactionsByYear> TransactionsByYears { get; set; }
}