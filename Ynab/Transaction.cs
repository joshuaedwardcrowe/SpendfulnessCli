using Ynab.Responses.Transactions;
using Ynab.Sanitisers;

namespace Ynab;

public class Transaction(TransactionResponse transactionResponse)
{
    private readonly TransactionResponse _transactionResponse = transactionResponse;
    public DateTime Occured => _transactionResponse.Occured;
    public string? Memo => _transactionResponse.Memo;
    public decimal Amount => MilliunitSanitiser.Calculate(_transactionResponse.Amount);
    public string? FlagName => _transactionResponse.FlagName;
    public FlagColor? FlagColour => _transactionResponse.FlagColor;
    public string PayeeName => _transactionResponse.PayeeName;
    public Guid? CategoryId => _transactionResponse.CategoryId;
    public string CategoryName => _transactionResponse.CategoryName;
    public bool IsTransfer => !string.IsNullOrEmpty(_transactionResponse.TransferTransactionId);
}