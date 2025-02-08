using Ynab;
using Ynab.Collections;
using Ynab.Sanitisers;

namespace YnabProgressConsole.Compilation.RecurringTransactions;

public class TransactionsByMemoOccurrenceByPayeeNameViewModelBuilder
    : IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>
{
    private List<TransactionsByMemoOccurrenceByPayeeName> _groups = [];
    private List<string> _columnNames = [];
    private string? _sortOnColumnName;
    private SortOrder _sortOrder;

    public IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> AddGroups(
        IEnumerable<TransactionsByMemoOccurrenceByPayeeName> groups)
    {
        _groups = groups.ToList();
        return this;
    }

    private void ValidateColumnNames(params string[] columnNames)
    {
        var validColumnNames = TransactionsByMemoOccurrenceByPayeeNameViewModel.GetColumnNames();
        
        var invalidColumnNames = columnNames
            .Where(columnName => !validColumnNames.Contains(columnName));

        if (invalidColumnNames.Any())
        {
            var invalidColumnNamesString = string.Join(", ", invalidColumnNames);
            throw new ArgumentException($"Invalid column names: {invalidColumnNamesString}");
        }
    }
    
    public IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> AddColumnNames(params string[] columnNames)
    {
        ValidateColumnNames(columnNames);
        
        _columnNames = columnNames.ToList();
        return this;
    }

    public IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> AddSortColumnName(string columnName)
    {
        ValidateColumnNames(columnName);
        
        _sortOnColumnName = columnName;
        return this;
    }

    public IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> AddSortOrder(SortOrder sortOrder)
    {
        _sortOrder = sortOrder;
        return this;
    }

    public ViewModel Build()
    {
        var sortColumnIndex = _columnNames.IndexOf(_sortOnColumnName);
        
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