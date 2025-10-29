using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.CliTables.Formatters;
using Spendfulness.Cli.CliTables.ViewModels;

namespace Spendfulness.Cli.CliTables.ViewModelBuilders;

public class TransactionYearAverageCliTableBuilder : CliTableBuilder<IEnumerable<TransactionYearAverageAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<TransactionYearAverageAggregate> evaluation)
        => TransactionYearAverageViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<TransactionYearAverageAggregate> aggregates)
    {
        var rows = BuildMultipleRows(aggregates);

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