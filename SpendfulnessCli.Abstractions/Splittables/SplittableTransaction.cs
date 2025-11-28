using Ynab;
using Ynab.Responses.Transactions;

// TODO: Move to Spendfulness, this stuff isnt Cli specific. 
namespace SpendfulnessCli.Abstractions.Splittables;

/// <summary>
/// A Transaction that has multiple notes in the Memo.
/// </summary>
public class SplittableTransaction(TransactionResponse transactionResponse) : Transaction(transactionResponse)
{
    /// <summary>
    /// Get the list of notes in the Memo.
    /// </summary>
    public string[] MemoNotes => Memo?.Split(',') ?? [];
}