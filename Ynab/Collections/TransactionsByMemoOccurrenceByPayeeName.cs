namespace Ynab.Collections;

public class TransactionsByMemoOccurrenceByPayeeName
{
    public required string PayeeName { get; set; }
    public required IEnumerable<TransactionsByMemoOccurrence> TransactionsByMemoOccurences { get; set; }
}