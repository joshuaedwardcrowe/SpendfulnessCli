using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Calculators;
using Ynab;
using Ynab.Extensions;

namespace Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;

public class TransactionMonthTotalYnabAggregator(IEnumerable<Transaction> transactions)
    : ListYnabAggregator<TransactionMonthTotalAggregate>(transactions)
{
    protected override IEnumerable<TransactionMonthTotalAggregate> ListAggregate()
    {
        var transactionsGroupedByMonth = Transactions
            .GroupByMonth()
            .ToList();
        
        for (var i = 0; i < transactionsGroupedByMonth.Count; i++)
        {
            // 1, 1, 2, 3, 4
            var priorIndex = i > 0 ? i - 1 : 0;
            
            var priorAverage = transactionsGroupedByMonth[priorIndex]
                .Transactions
                .Sum(x => x.Amount);

            var currentAverage = transactionsGroupedByMonth[i]
                .Transactions.Sum(m => m.Amount);
            
            var percentageChange = PercentageCalculator.CalculateChange(priorAverage, currentAverage);
            
            yield return new TransactionMonthTotalAggregate(
                transactionsGroupedByMonth[i].Month, 
                currentAverage,
                percentageChange);
        }
    }
}