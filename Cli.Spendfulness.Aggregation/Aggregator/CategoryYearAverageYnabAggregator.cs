using Cli.Spendfulness.Aggregation.Aggregates;
using Ynab;
using Ynab.Extensions;
using Cli.Spendfulness.Aggregation.Extensions;

namespace Cli.Spendfulness.Aggregation.Aggregator;

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