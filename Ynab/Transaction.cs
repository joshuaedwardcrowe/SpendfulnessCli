using Ynab.Responses.Transactions;
using Ynab.Sanitisers;

namespace Ynab;

public class Transaction(TransactionResponse transactionResponse)
{
    public DateTime Occured => transactionResponse.Occured;
    public string? Memo => transactionResponse.Memo;
    public decimal Amount => MilliunitSanitiser.Calculate(transactionResponse.Amount);
    public string? FlagName => transactionResponse.FlagName;
    public FlagColor? FlagColour => transactionResponse.FlagColor;
    public string PayeeName => transactionResponse.PayeeName;
    public Guid? CategoryId => transactionResponse.CategoryId;
    public string CategoryName => transactionResponse.CategoryName;
    public bool IsTransfer => !string.IsNullOrEmpty(transactionResponse.TransferTransactionId);
}