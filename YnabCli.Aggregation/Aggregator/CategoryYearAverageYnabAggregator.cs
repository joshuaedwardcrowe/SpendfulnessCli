using Ynab;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Extensions;

namespace YnabCli.Aggregation.Aggregator;

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