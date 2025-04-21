using Ynab.Responses.Transactions;

namespace Ynab;

public class Transaction(TransactionResponse transactionResponse) : SubTransaction(transactionResponse)
{
    public DateTime Occured => transactionResponse.Occured;
    public string? FlagName => transactionResponse.FlagName;
    public FlagColor? FlagColour => transactionResponse.FlagColor;
    public IEnumerable<SubTransaction> SubTransactions => transactionResponse.SubTransactions
        .Select(transaction => new SubTransaction(transaction));
}