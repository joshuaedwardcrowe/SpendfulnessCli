namespace Ynab.Collections;

// TODO: In order to move this out of the Ynab communication project, 
public class TransactionsForFlag
{
    public required string Flag { get; set; }
    public required List<Transaction> Transactions { get; set; }
}