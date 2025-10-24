using Ynab.Sanitisers;
using Cli.Spendfulness.Aggregation.Aggregates;
using Cli.Spendfulness.CliTables.Formatters;
using Cli.Spendfulness.CliTables.ViewModels;

namespace Cli.Spendfulness.CliTables.ViewModelBuilders;

public class TransactionPayeeMemoOccurrenceCliTableBuilder : 
    CliTableBuilder<IEnumerable<TransactionPayeeMemoOccurrenceAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<TransactionPayeeMemoOccurrenceAggregate> evaluation)
        => TransactionMemoOccurrenceTable.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<TransactionPayeeMemoOccurrenceAggregate> aggregates)
        => aggregates
            .Select(BuildMemoOccurrenceRow)
            .ToList();

    private List<object> BuildMemoOccurrenceRow(TransactionPayeeMemoOccurrenceAggregate aggregate)
    {
        var flowSanitisedAverageAmount = TransactionFlowSanitiser.Sanitise(aggregate.AverageAmount);
        var displayableAverageAmount = CurrencyDisplayFormatter.Format(flowSanitisedAverageAmount);
        
        var flowSanitisedTotalAmount = TransactionFlowSanitiser.Sanitise(aggregate.TotalAmount);
        var displayableTotalAmount = CurrencyDisplayFormatter.Format(flowSanitisedTotalAmount);

        return
        [
            aggregate.PayeeName,
            aggregate.Memo ?? string.Empty,
            aggregate.MemoOccurrence,
            displayableAverageAmount,
            displayableTotalAmount
        ];
    }
}