namespace Ynab.Collections;

public class TransactionsByMemoOccurrenceByPayeeName
{
    public string PayeeName { get; set; }
    public IEnumerable<TransactionsByMemoOccurrence> TransactionsByMemoOccurences { get; set; } = new List<TransactionsByMemoOccurrence>();
}