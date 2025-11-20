using SpendfulnessCli.Abstractions.Calculators;
using SpendfulnessCli.Aggregation.Aggregates;
using Ynab;
using Ynab.Extensions;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

public class TransactionAverageAcrossYearYnabAggregator(IEnumerable<Transaction> transactions)
    : ListYnabAggregator<TransactionYearAverageAggregate>(transactions)
{
    protected override IEnumerable<TransactionYearAverageAggregate> ListAggregate()
    {
        var transactionsGroupedByYear = Transactions.GroupByYear().ToList();
        
        for (var i = 0; i < transactionsGroupedByYear.Count; i++)
        {
            var year = transactionsGroupedByYear[i].Year;
            
            // 1, 1, 2, 3, 4
            var priorIndex = i > 1 ? i - 1 : 1;

            var priorAverage = transactionsGroupedByYear
                .Take(priorIndex)
                .Average(t => t.Transactions.Sum(m => m.Amount));

            var currentAverage = transactionsGroupedByYear
                .Take(i + 1)
                .Average(t => t.Transactions.Sum(m => m.Amount));
            
            var percentageChange = PercentageCalculator.CalculateChange(priorAverage, currentAverage);
            
            yield return new TransactionYearAverageAggregate(year, currentAverage, percentageChange);
        }
    }
}