using Ynab;
using Ynab.Extensions;
using SpendfulnessCli.Aggregation.Extensions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Aggregation.Aggregator;

// TODO: This should be a list aggregator.

public class CategoryYearAverageYnabAggregator(IEnumerable<Transaction> transactions)
    : YnabAggregator<IEnumerable<CategoryYearAverageAggregate>>(transactions)
{
    protected override IEnumerable<CategoryYearAverageAggregate> GenerateAggregate()
         => Transactions
                .GroupByCategory()
                .GroupByYear()
                .AggregateYearAverages();
}