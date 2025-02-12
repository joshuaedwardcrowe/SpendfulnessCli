using Ynab.Sanitisers;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.Extensions;
using YnabProgressConsole.Compilation.Formatters;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public class TransactionMemoOccurrenceViewModelBuilder : ViewModelBuilder, ITransactionMemoOccurrenceViewModelBuilder
{
    private TransactionMemoOccurrenceEvaluator _evaluator = new();
    private string? _payeeNameFilter = null;
    private int? _minimumOccurrencesFilter = null;
    
    public IEvaluationViewModelBuilder<TransactionMemoOccurrenceEvaluator, IEnumerable<TransactionMemoOccurrenceAggregate>> AddEvaluator(TransactionMemoOccurrenceEvaluator evaluator)
    {
        _evaluator = evaluator;
        return this;
    }

    public ITransactionMemoOccurrenceViewModelBuilder AddPayeeNameFilter(string payeeNameFilter)
    {
        _payeeNameFilter = payeeNameFilter;
        return this;
    }

    public ITransactionMemoOccurrenceViewModelBuilder AddMinimumOccurrencesFilter(int minimumOccurrences)
    {
        _minimumOccurrencesFilter = minimumOccurrences;
        return this;
    }
    
    public ViewModel Build()
    {
        var evaluatedOccurrences = _evaluator.Evaluate();

        if (_payeeNameFilter != null)
        {
            evaluatedOccurrences = evaluatedOccurrences
                .FilterByPayeeName(_payeeNameFilter);
        }

        if (_minimumOccurrencesFilter.HasValue)
        {
            evaluatedOccurrences = evaluatedOccurrences
                .FilterByMinimumOccurrences(_minimumOccurrencesFilter.Value);
        }
        
        var rows = evaluatedOccurrences
            .OrderBySortOrder(ViewModelSortOrder, aggregate => aggregate.MemoOccurrence)
            .Select(BuildMemoOccurrenceRow)
            .ToList();
        
        return new TransactionMemoOccurrenceViewModel
        {
            Columns = ColumnNames,
            Rows = rows
        };
    }
    
    private List<object> BuildMemoOccurrenceRow(TransactionMemoOccurrenceAggregate aggregate)
    {
        var averageAmount = BuildAverageAmount(aggregate.AverageAmount);

        return
        [
            aggregate.PayeeName,
            aggregate.Memo,
            aggregate.MemoOccurrence,
            averageAmount
        ];
    }

    private static string BuildAverageAmount(decimal averageAmount)
    {
        var flowSanitisedAmount = TransactionFlowSanitiser.Sanitise(averageAmount);
        return CurrencyDisplayFormatter.Format(flowSanitisedAmount);
    }
}