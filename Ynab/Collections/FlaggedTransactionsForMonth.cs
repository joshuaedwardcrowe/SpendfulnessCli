namespace Ynab.Collections;

public class FlaggedTransactionsForMonth
{
    public required string Month { get; set; }
    public required List<TransactionsForFlag> TransactionsForFlags { get; set; }
}