using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Aggregation.Extensions;
using YnabSharp;
using YnabSharp.Extensions;

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