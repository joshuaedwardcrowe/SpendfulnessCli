using Ynab.Sanitisers;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Extensions;
using YnabProgressConsole.Compilation.Formatters;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class TransactionMemoOccurrenceViewModelBuilder : 
    ViewModelBuilder<TransactionMemoOccurrenceAggregator, IEnumerable<TransactionMemoOccurrenceAggregate>>
{
    private string? _payeeNameFilter;
    private int? _minimumOccurrencesFilter;
    
    public TransactionMemoOccurrenceViewModelBuilder AddPayeeNameFilter(string payeeNameFilter)
    {
        _payeeNameFilter = payeeNameFilter;
        return this;
    }

    public TransactionMemoOccurrenceViewModelBuilder AddMinimumOccurrencesFilter(int minimumOccurrences)
    {
        _minimumOccurrencesFilter = minimumOccurrences;
        return this;
    }
    
    protected override List<List<object>> BuildRows(IEnumerable<TransactionMemoOccurrenceAggregate> occurrences)
    {
         if (_payeeNameFilter != null)
         {
             occurrences = occurrences
                 .FilterByPayeeName(_payeeNameFilter);
         }

         if (_minimumOccurrencesFilter.HasValue)
         {
             occurrences = occurrences
                 .FilterByMinimumOccurrences(_minimumOccurrencesFilter.Value);
         } 
         
         return occurrences
             .OrderBySortOrder(ViewModelSortOrder, aggregate => aggregate.MemoOccurrence)
             .Select(BuildMemoOccurrenceRow)
             .ToList();
    }
    
    private List<object> BuildMemoOccurrenceRow(TransactionMemoOccurrenceAggregate aggregate)
    {
        var flowSanitisedAmount = TransactionFlowSanitiser.Sanitise(aggregate.AverageAmount);
        var displayableAverageAmount = CurrencyDisplayFormatter.Format(flowSanitisedAmount);

        return
        [
            aggregate.PayeeName,
            aggregate.Memo ?? string.Empty,
            aggregate.MemoOccurrence,
            displayableAverageAmount
        ];
    }
}