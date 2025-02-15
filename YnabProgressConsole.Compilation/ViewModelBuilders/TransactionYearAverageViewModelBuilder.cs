using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class TransactionYearAverageViewModelBuilder :
    ViewModelBuilder<TransactionYearAverageEvaluator, IEnumerable<TransactionYearAverageAggregate>>
{
    protected override List<List<object>> BuildRows(TransactionYearAverageEvaluator evaluator)
    {
        var transactionYearAverageAggregates = evaluator.Evaluate();
        var rows = BuildRows(transactionYearAverageAggregates);

        return rows.ToList();
    }
    
    private IEnumerable<List<object>> BuildRows(IEnumerable<TransactionYearAverageAggregate> transactionYearAverages)
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