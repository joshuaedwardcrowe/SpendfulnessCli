using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class TransactionYearAverageViewModelBuilder :
    ViewModelBuilder<TransactionYearAverageAggregator, IEnumerable<TransactionYearAverageAggregate>>
{
    protected override List<List<object>> BuildRows(IEnumerable<TransactionYearAverageAggregate> evaluation)
    {
        var rows = BuildMultipleRows(evaluation);

        return rows.ToList();
    }
    
    private IEnumerable<List<object>> BuildMultipleRows(IEnumerable<TransactionYearAverageAggregate> transactionYearAverages)
    {
        foreach (var transactionYearAverage in transactionYearAverages)
        {
            var displayableAverage = CurrencyDisplayFormatter.Format(transactionYearAverage.AverageAmount);
            var displayablePercentage = PercentageDisplayFormatter.Format(transactionYearAverage.PercentageChange);

            yield return
            [
                transactionYearAverage.Year,
                displayableAverage,
                displayablePercentage
            ];
        }
    }
}