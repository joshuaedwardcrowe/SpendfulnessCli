namespace Ynab.Collections;

public class TransactionsByMemoOccurrence
{
    public required string? Memo { get; set; }
    public required int MemoOccurence { get; set; }
    public required IEnumerable<Transaction> Transactions { get; set; }
}