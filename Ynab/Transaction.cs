using Ynab.Calculators;
using Ynab.Responses.Transactions;

namespace Ynab;

public class Transaction(TransactionResponse transactionResponse)
{
    private TransactionResponse _transactionResponse = transactionResponse;

    public DateTime Occured => _transactionResponse.Occured;
    public string Memo => _transactionResponse.Memo;
    public decimal Amount => MilliunitCalculator.Calculate(_transactionResponse.Amount);
    public string? FlagName => _transactionResponse.FlagName;
    public string? FlagColour => _transactionResponse.FlagColor;
    public string PayeeName => _transactionResponse.PayeeName;
    public Guid? CategoryId => _transactionResponse.CategoryId;
}