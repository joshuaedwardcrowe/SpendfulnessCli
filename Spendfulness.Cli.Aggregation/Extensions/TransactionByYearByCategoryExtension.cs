using Spendfulness.Cli.Aggregation.Aggregates;
using Ynab.Collections;

namespace Spendfulness.Cli.Aggregation.Extensions;

public static class TransactionByYearByCategoryExtension
{
    public static IEnumerable<CategoryYearAverageAggregate> AggregateYearAverages(
        this IEnumerable<TransactionsByYearByCategory> transactionGroups)
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