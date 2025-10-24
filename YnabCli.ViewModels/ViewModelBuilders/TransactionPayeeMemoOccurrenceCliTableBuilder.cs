using Ynab.Sanitisers;
using YnabCli.Aggregation.Aggregates;
using YnabCli.ViewModels.Formatters;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

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