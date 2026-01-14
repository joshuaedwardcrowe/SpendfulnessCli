using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Aggregation.Extensions;
using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;
using Ynab.Extensions;

namespace Spendfulness.Aggregation.Aggregator;

// TODO: FIx this name.
public class CategoryYearAverageYnabListAggregator(IEnumerable<Transaction> transactions)
    : YnabListAggregator<CategoryYearAverageAggregate>(transactions)
{
    protected override IEnumerable<CategoryYearAverageAggregate> GenerateAggregate()
         => Transactions
                .GroupByCategory()
                .GroupByYear()
                .AggregateYearAverages();
}