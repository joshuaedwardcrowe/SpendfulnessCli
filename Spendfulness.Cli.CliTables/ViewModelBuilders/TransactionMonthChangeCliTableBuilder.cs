using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.CliTables.Formatters;
using Spendfulness.Cli.CliTables.ViewModels;

namespace Spendfulness.Cli.CliTables.ViewModelBuilders;

public class TransactionMonthChangeCliTableBuilder : CliTableBuilder<IEnumerable<TransactionMonthTotalAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<TransactionMonthTotalAggregate> evaluation)
        => TransactionMonthChangeTable.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<TransactionMonthTotalAggregate> aggregates)
        => aggregates
            .Select(BuildIndividualRow)
            .Select(row => row.ToList())
            .ToList();
    
    private IEnumerable<object> BuildIndividualRow(TransactionMonthTotalAggregate aggregate)
    {
        yield return aggregate.Month;
        
        var displayableTotalAmount = CurrencyDisplayFormatter.Format(aggregate.TotalAmount);
        yield return displayableTotalAmount;

        var displayablePercentageChangeAmount = PercentageDisplayFormatter.Format(aggregate.PercentageChange);
        yield return displayablePercentageChangeAmount;
    }
}