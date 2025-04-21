using Ynab;
using Ynab.Collections;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;

namespace YnabCli.Aggregation.Aggregator;

// TODO: This should be a list aggregator.

[Obsolete("Please do not use this method.")]
public class LegacyCategoryYearAverageAggregator(IEnumerable<Transaction> transactions)
    : Aggregator<IEnumerable<CategoryYearAverageAggregate>>(transactions)
{
    protected override IEnumerable<CategoryYearAverageAggregate> GenerateAggregate()
    {
        var transactionGroups = Transactions
            .GroupByCategory()
            .GroupByYear();
        
        return MapToAggregate(transactionGroups);
    }

    // TODO: I wonder if this could be an extension...
    private IEnumerable<CategoryYearAverageAggregate> MapToAggregate(
        IEnumerable<TransactionsByYearByCategory> transactionGroups)
    {
        foreach (var transactionGroup in transactionGroups)
        {
            var averageAmountByYears = transactionGroup
                .TransactionsByYear
                .ToDictionary(
                    transactionByYear => transactionByYear.Year,
                    transactionByYear => transactionByYear.Transactions.Average(t => t.Amount));

            yield return new CategoryYearAverageAggregate(transactionGroup.CategoryName, averageAmountByYears);
        }
    }
}