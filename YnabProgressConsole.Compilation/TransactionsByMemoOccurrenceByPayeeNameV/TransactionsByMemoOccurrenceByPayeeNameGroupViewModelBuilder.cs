using Ynab;
using Ynab.Collections;
using Ynab.Sanitisers;

namespace YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameV;

public class TransactionsByMemoOccurrenceByPayeeNameGroupViewModelBuilder
    : ViewModelBuilder, IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>
{
    private List<TransactionsByMemoOccurrenceByPayeeName> _groups = [];

    public IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> AddGroups(
        IEnumerable<TransactionsByMemoOccurrenceByPayeeName> groups)
    {
        _groups = groups.ToList();
        return this;
    }
    
    public ViewModel Build()
    {
        var sortColumnIndex = _columnNames.IndexOf(_sortColumnName);
        
        var rows = BuildRows(_groups);
        var orderedRows = OrderRows(rows, sortColumnIndex, _sortOrder);
        
        return new TransactionsByMemoOccurrenceByPayeeNameViewModel
        {
            Columns = _columnNames,
            Rows = orderedRows.ToList()
        };
    }

    private IEnumerable<List<object>> BuildRows(
        List<TransactionsByMemoOccurrenceByPayeeName> groupCollection)
            => groupCollection
                .SelectMany(group => BuildMemoOccurrenceRows(
                    group.PayeeName, group.TransactionsByMemoOccurences));

    private IOrderedEnumerable<List<object>> OrderRows(
        IEnumerable<List<object>> rows, int sortColumnIndex, SortOrder sortOrder)
            => sortOrder == SortOrder.Ascending
                ? rows.OrderBy(row => row[sortColumnIndex])
                : rows.OrderByDescending(row => row[sortColumnIndex]);

    private IEnumerable<List<object>> BuildMemoOccurrenceRows(
        string payeeName, IEnumerable<TransactionsByMemoOccurrence> transactionsByMemoOccurrences)
    {
        foreach (var transactionsByMemoOccurrence in transactionsByMemoOccurrences)
        {
            var averageAmount = BuildAverageAmount(transactionsByMemoOccurrence.Transactions);

            yield return
            [
                payeeName,
                transactionsByMemoOccurrence?.Memo ?? string.Empty,
                transactionsByMemoOccurrence.MemoOccurence,
                averageAmount
            ];
        }
    }

    private string BuildAverageAmount(IEnumerable<Transaction> memoOccurrences)
    {
        var averageAmount = memoOccurrences.Average(transaction => transaction.Amount);
        var decimalSanitisedAmount = DecimalPlaceSanitiser.Sanitise(averageAmount);
        var flowSanitisedAmount = TransactionFlowSanitiser.Sanitise(decimalSanitisedAmount);

        return $"£{flowSanitisedAmount}";
    }
}