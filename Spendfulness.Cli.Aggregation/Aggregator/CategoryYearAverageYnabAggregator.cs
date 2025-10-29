using Ynab;
using Ynab.Extensions;
using Spendfulness.Cli.Aggregation.Extensions;
using Spendfulness.Cli.Aggregation.Aggregates;

namespace Spendfulness.Cli.Aggregation.Aggregator;

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