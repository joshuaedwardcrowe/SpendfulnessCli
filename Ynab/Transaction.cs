using Ynab.Responses.Transactions;

namespace Ynab;

public class Transaction(TransactionResponse transactionResponse) : SplitTransactions(transactionResponse)
{
    public DateTime Occured => transactionResponse.Occured;
    public string? FlagName => transactionResponse.FlagName;
    public FlagColor? FlagColour => transactionResponse.FlagColor;
    public IEnumerable<SplitTransactions> SplitTransactions => transactionResponse.SplitTransactions
        .Select(transaction => new SplitTransactions(transaction));
}