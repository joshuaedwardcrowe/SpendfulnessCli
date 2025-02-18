using Ynab;
using Ynab.Collections;
using Ynab.Extensions;
using YnabCli.ViewModels.Aggregates;

namespace YnabCli.ViewModels.Aggregator;

public class CategoryYearAverageAggregator(IEnumerable<Transaction> transactions)
    : Aggregator<IEnumerable<CategoryYearAverageAggregate>>(transactions)
{
    public override IEnumerable<CategoryYearAverageAggregate> Aggregate()
    {
        var transactionGroups = Transactions
            .GroupByCategory()
            .GroupByYear();
        
        return MapToAggregate(transactionGroups);
    }

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