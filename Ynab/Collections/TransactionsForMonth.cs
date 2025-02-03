namespace Ynab.Collections;

public class TransactionsForMonth
{
    public required string Month { get; set; }
    public required IEnumerable<Transaction> Transactions { get; set; }
}