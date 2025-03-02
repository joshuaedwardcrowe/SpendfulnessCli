using Ynab.Sanitisers;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.Extensions;
using YnabCli.ViewModels.Formatters;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

public class TransactionMemoOccurrenceViewModelBuilder : 
    ViewModelBuilder<ListAggregator<TransactionMemoOccurrenceAggregate>, IEnumerable<TransactionMemoOccurrenceAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<TransactionMemoOccurrenceAggregate> evaluation)
        => TransactionMemoOccurrenceViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<TransactionMemoOccurrenceAggregate> aggregates)
        => aggregates
            .Select(BuildMemoOccurrenceRow)
            .ToList();

    private List<object> BuildMemoOccurrenceRow(TransactionMemoOccurrenceAggregate aggregate)
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