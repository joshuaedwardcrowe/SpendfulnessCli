using Ynab;
using Ynab.Collections;
using Ynab.Connected;
using Ynab.Extensions;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Calculators;

namespace YnabCli.ViewModels.Aggregator;

public class TransactionYearAverageAggregator(IEnumerable<Transaction> transactions)
    : Aggregator<IEnumerable<TransactionYearAverageAggregate>>(transactions)
{
    public override IEnumerable<TransactionYearAverageAggregate> Aggregate()
    {
        var transactionsGroupedByYear = Transactions
            .FilterToInflow()
            .FilterByPayeeName("BrightHR")
            .OrderByYear()
            .GroupByYear()
            .ToList();
        
        var firstGroup = AggregateFirstGroup(transactionsGroupedByYear);
        var remainingRows = AggregateRemainingGroups(transactionsGroupedByYear);
        
        return new List<TransactionYearAverageAggregate> { firstGroup }.Concat(remainingRows);
    }

    private TransactionYearAverageAggregate AggregateFirstGroup(List<TransactionsByYear> transactionGroups)
    {
        var firstGroup = transactionGroups.FirstOrDefault();
        if (firstGroup == null)
        {
            throw new Exception("Could not do this");
        }
        
        var averageAmount = firstGroup.Transactions.Average(x => x.Amount);
        return new TransactionYearAverageAggregate(firstGroup.Year, averageAmount, 0);
    }

    private IEnumerable<TransactionYearAverageAggregate> AggregateRemainingGroups(List<TransactionsByYear> transactionGroups)
    {
        // We skip so when comparing second row we have a prior row to compare with
        for (var i = 1; i < transactionGroups.Count; i++)
        {
            var indexOfPriorGroup = i - 1;
            
            var priorGroup = transactionGroups.ElementAt(indexOfPriorGroup);
            var currentGroup = transactionGroups.ElementAt(i);

            var priorGroupAverage = priorGroup.Transactions.Average(x => x.Amount);
            var currentGroupAverage = currentGroup.Transactions.Average(x => x.Amount);
            
            var percentageChange = PercentageCalculator.CalculateChange(
                currentGroupAverage,
                priorGroupAverage);
            
            yield return new TransactionYearAverageAggregate(currentGroup.Year, currentGroupAverage, percentageChange);
        }
    } 
}