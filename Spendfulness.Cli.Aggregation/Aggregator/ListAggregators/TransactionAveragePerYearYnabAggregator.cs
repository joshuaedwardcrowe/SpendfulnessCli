using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Calculators;
using Ynab;
using Ynab.Collections;
using Ynab.Extensions;

namespace Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;

public class TransactionAveragePerYearYnabAggregator(IEnumerable<Transaction> transactions)
    : ListYnabAggregator<TransactionYearAverageAggregate>(transactions)
{
    protected override IEnumerable<TransactionYearAverageAggregate> ListAggregate()
    {
        var transactionsGroupedByYear = Transactions
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
            // This is genuinely exceptional circumstances.
            throw new InvalidOperationException("No first group found");
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
            
            var percentageChange = PercentageCalculator.CalculateChange(priorGroupAverage, currentGroupAverage);
            
            yield return new TransactionYearAverageAggregate(currentGroup.Year, currentGroupAverage, percentageChange);
        }
    } 
}