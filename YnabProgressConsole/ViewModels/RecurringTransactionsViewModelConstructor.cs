using Ynab.Collections;
using Ynab.Sanitisers;

namespace YnabProgressConsole.ViewModels;

public class RecurringTransactionsViewModelConstructor
    : IViewModelConstructor<IEnumerable<TransactionsByMemoOccurrenceByPayeeName>>
{
    public ViewModel Construct(IEnumerable<TransactionsByMemoOccurrenceByPayeeName> groupCollection)
    {
        var allRows = ConstructAllRows(groupCollection);
        
        var orderedRows = allRows
            .OrderByDescending(rowColumn => rowColumn[2])
            .ToList();

        return new ViewModel
        {
            Columns = ["Payee", "Memo", "Occurrence", "Average Spend"],
            Rows = orderedRows
        };
    }

    private IEnumerable<List<object>> ConstructAllRows(
        IEnumerable<TransactionsByMemoOccurrenceByPayeeName> groupCollection)
    {
        var allRows = new List<List<object>>();

        foreach (var group in groupCollection)
        {
            var groupRows = ConstructGroupRows(group);
            allRows.AddRange(groupRows);
        }
        
        return allRows;
    }

    private IEnumerable<List<object>> ConstructGroupRows(TransactionsByMemoOccurrenceByPayeeName group)
    {
        foreach (var subGroup in group.TransactionsByMemoOccurences)
        {
            var averageSpend = subGroup.Transactions.Average(transaction => transaction.Amount);
            var sanitisedAverageSpend = DecimalSanitiser.Sanitise(averageSpend);

            yield return
            [
                group.PayeeName,
                subGroup.Memo,
                subGroup.MemoOccurence,
                sanitisedAverageSpend
            ];
        }
    }
}
