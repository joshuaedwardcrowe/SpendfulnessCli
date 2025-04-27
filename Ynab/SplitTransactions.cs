using Ynab.Responses.Transactions;
using Ynab.Sanitisers;

namespace Ynab;

// TODO: Hate that this is called 'sub'
public class SplitTransactions(SplitTransactionResponse splitTransactionResponse)
{
    public string Id => splitTransactionResponse.Id;
    public string? Memo => splitTransactionResponse.Memo;
    public decimal Amount => MilliunitSanitiser.Calculate(splitTransactionResponse.Amount);
    public Guid? PayeeId => splitTransactionResponse.PayeeId;
    public string PayeeName => splitTransactionResponse.PayeeName;
    public Guid? CategoryId => splitTransactionResponse.CategoryId;
    public string CategoryName => splitTransactionResponse.CategoryName;
    public bool IsTransfer => !string.IsNullOrEmpty(splitTransactionResponse.TransferTransactionId);
    
    public bool InCategories(IEnumerable<Guid> categoryIds)
        => CategoryId.HasValue && categoryIds.Contains(CategoryId.Value);
}